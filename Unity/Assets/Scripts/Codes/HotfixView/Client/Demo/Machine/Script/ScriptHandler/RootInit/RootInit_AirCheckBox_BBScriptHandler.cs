using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.AirCheckTimer)]
    [FriendOf(typeof(B2Unit))]
    [FriendOf(typeof(BBParser))]
    public class AirCheckTimer : BBTimer<Unit>
    {
        protected override void Run(Unit self)
        {
            B2Unit b2Unit = self.GetComponent<B2Unit>(); // 碰撞信息缓存组件
            BehaviorMachine machine = self.GetComponent<BehaviorMachine>(); // 控制器逻辑组件
    
            Queue<CollisionInfo> infoQueue = b2Unit.TriggerBuffer;
            int count = infoQueue.Count;
            while (count-- > 0)
            {
                CollisionInfo info = infoQueue.Dequeue();
                infoQueue.Enqueue(info);
    
                if (info.dataA.Name.Equals("AirCheckBox") && info.dataB.LayerMask is LayerType.Ground && !info.dataB.IsTrigger)
                {
                    if (machine.GetParam<bool>("InAir"))
                    {
                        machine.UpdateParam("InAir", false);
                        //落地回调
                        EventSystem.Instance.Invoke(new LandCallback() { instanceId = self.InstanceId });
                    }
                    return;
                }
            }
    
            machine.UpdateParam("InAir", true);
        }
    }
    
    [FriendOf(typeof(BehaviorMachine))]
    public class RootInit_AirCheckBox_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AirCheckBox";
        }

        // GroundCheckBox: 0, -2200, 2000, 1000; (Center), (Size)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"AirCheckBox: (?<CenterX>-?\d+), (?<CenterY>-?\d+), (?<SizeX>-?\d+), (?<SizeY>-?\d+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["CenterX"].Value, out long centerX) ||
                !long.TryParse(match.Groups["CenterY"].Value, out long centerY) ||
                !long.TryParse(match.Groups["SizeX"].Value, out long sizeX) ||
                !long.TryParse(match.Groups["SizeY"].Value, out long sizeY))
            {
                Log.Error($"cannot format {match.Groups["SizeX"]} / {match.Groups["SizeY"]} / {match.Groups["CenterX"]} / {match.Groups["CenterY"]} to long!! ");
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);

            //1. 注册变量
            machine.RegistParam("InAir", false);

            //2. 创建夹具
            PolygonShape shape = new();
            shape.SetAsBox(sizeX / 2000f, sizeY / 2000f, new Vector2(centerX, centerY) / 1000f, 0f);
            FixtureDef fixtureDef = new()
            {
                Shape = shape,
                Density = 1.0f,
                Friction = 0.0f,
                UserData = new FixtureData()
                {
                    InstanceId = body.InstanceId,
                    Name = "AirCheckBox",
                    Type = FixtureType.Default,
                    LayerMask = LayerType.Unit,
                    IsTrigger = true,
                    UserData = new BoxInfo()
                    {
                        boxName = "AirCheckBox",
                        center = new UnityEngine.Vector2(centerX, centerY) / 1000f,
                        size = new UnityEngine.Vector2(sizeX, sizeY) / 1000f,
                        hitboxType = HitboxType.Other
                    },
                    TriggerStayId = TriggerStayType.CollisionEvent
                }
            };
            Fixture fixture = body.CreateFixture(fixtureDef);
            machine.RegistParam("AirCheckBox", fixture);
            
            //3. 创建定时器
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            long timer = postStepTimer.NewFrameTimer(BBTimerInvokeType.AirCheckTimer, unit);
            machine.RegistParam("AirCheckTimer", timer);
            //热更新 or 销毁unit时，销毁定时器
            machine.Token.Add(() =>
            {
                if (machine.ContainParam("AirCheckTimer"))
                {
                    long _timer = machine.GetParam<long>("AirCheckTimer");
                    b2WorldManager.Instance.GetPostStepTimer().Remove(ref _timer);
                    machine.TryRemoveParam("AirCheckTimer");
                }
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}