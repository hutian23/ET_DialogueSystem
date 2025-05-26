using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(EventType.TimeFrozeCheckTimer)]
    // [FriendOf(typeof(B2Unit))]
    // [FriendOf(typeof(b2Body))]
    public class TimeFrozeCheckTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            // B2Unit b2Unit = self.GetParent<Unit>().GetComponent<B2Unit>();
            // BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            // b2Body body = b2WorldManager.Instance.GetBody(self.GetParent<Unit>().InstanceId);
            //
            // int lastFrame = self.GetParam<int>("TimeFroze_LastFrame");
            // int hertz = self.GetParam<int>("TimeFroze_Hertz");
            // self.UpdateParam("TimeFroze_LastFrame", --lastFrame);
            //
            // Queue<CollisionInfo> infoQueue = b2Unit.TriggerBuffer;
            // int count = infoQueue.Count;
            // while (count-- > 0)
            // {
            //     CollisionInfo info = infoQueue.Dequeue();
            //     infoQueue.Enqueue(info);
            //
            //     BoxInfo infoA = info.dataA.UserData as BoxInfo;
            //     BoxInfo infoB = info.dataB.UserData as BoxInfo;
            //     if (!info.dataA.Name.Equals("TimeFrozeCheckBox") ||
            //         infoA.hitboxType is not HitboxType.Other ||
            //         infoB.hitboxType is not HitboxType.Squash ||
            //         info.dataB.LayerMask is not LayerType.Unit || 
            //         info.dataB.InstanceId == 0)
            //     {
            //         continue;
            //     }
            //     
            //     b2Body bodyB = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
            //     Unit unit = Root.Instance.Get(bodyB.unitId) as Unit;
            //     BBNumeric numeric = unit.GetComponent<BBNumeric>();
            //     numeric.Set("Hertz", lastFrame <= 0? 60 : hertz);
            // }
            //
            // if (lastFrame > 0) return;
            //
            // //初始化
            // self.TryRemoveParam("TimeFroze_LastFrame");
            // self.TryRemoveParam("TimeFroze_Hertz");
            // if (self.ContainParam("TimeFroze_Timer"))
            // {
            //     long _timer = self.GetParam<long>("TimeFroze_Timer");
            //     postStepTimer.Remove(ref _timer);
            // }
            // self.TryRemoveParam("TimeFroze_Timer");
            // if (self.ContainParam("TimeFroze_CheckBox"))
            // {
            //     Fixture _fixture = self.GetParam<Fixture>("TimeFroze_CheckBox");
            //     body.DestroyFixture(_fixture);
            // }
            // self.TryRemoveParam("TimeFroze_CheckBox");
        }
    }

    public class TimeFrozeCheckBox_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "TimeFrozeCheckBox";
        }
        
        //TimeFrozeCheckBox: 0, 0, 200000, 100000, 4;(Center, Size, Hertz, LastFrame)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            // Match match = Regex.Match(data.opLine, @"TimeFrozeCheckBox: (?<CenterX>-?\d+), (?<CenterY>-?\d+), (?<SizeX>-?\d+), (?<SizeY>-?\d+), (?<Hertz>-?\d+), (?<LastFrame>-?\d+);");
            // if (!match.Success)
            // {
            //     ScriptHelper.ScripMatchError(data.opLine);
            //     return Status.Failed;
            // }
            //
            // if (!long.TryParse(match.Groups["CenterX"].Value, out long centerX) ||
            //     !long.TryParse(match.Groups["CenterY"].Value, out long centerY) ||
            //     !long.TryParse(match.Groups["SizeX"].Value, out long sizeX) ||
            //     !long.TryParse(match.Groups["SizeY"].Value, out long sizeY) || 
            //     !int.TryParse(match.Groups["LastFrame"].Value, out int lastFrame) ||
            //     !int.TryParse(match.Groups["Hertz"].Value, out int hertz))
            // {
            //     Log.Error($"cannot format {match.Groups["SizeX"]} / {match.Groups["SizeY"]} / {match.Groups["CenterX"]} / {match.Groups["CenterY"]} to long!! ");
            //     return Status.Failed;
            // }
            //
            // Unit unit = parser.GetParent<Unit>();
            // BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            // b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            //
            // //1. 初始化
            // parser.TryRemoveParam("TimeFroze_LastFrame");
            // parser.TryRemoveParam("TimeFroze_Hertz");
            // if (parser.ContainParam("TimeFroze_Timer"))
            // {
            //     long _timer = parser.GetParam<long>("TimeFroze_Timer");
            //     postStepTimer.Remove(ref _timer);
            // }
            // parser.TryRemoveParam("TimeFroze_Timer");
            // if (parser.ContainParam("TimeFroze_CheckBox"))
            // {
            //     Fixture _fixture = parser.GetParam<Fixture>("TimeFroze_CheckBox");
            //     body.DestroyFixture(_fixture);
            // }
            // parser.TryRemoveParam("TimeFroze_CheckBox");
            //
            // //1. 创建夹具
            // PolygonShape shape = new();
            // shape.SetAsBox(sizeX / 20000f, sizeY / 20000f, new Vector2(centerX, centerY) / 10000f, 0f);
            // FixtureDef fixtureDef = new()
            // {
            //     Shape = shape,
            //     Density = 1.0f,
            //     Friction = 0.0f,
            //     UserData = new FixtureData()
            //     {
            //         InstanceId = body.InstanceId,
            //         Name = "TimeFrozeCheckBox",
            //         Type = FixtureType.Default,
            //         LayerMask = LayerType.Unit,
            //         IsTrigger = true,
            //         UserData = new BoxInfo()
            //         {
            //             boxName = "TimeFrozeCheckBox",
            //             center = new UnityEngine.Vector2(centerX, centerY) / 10000f,
            //             size = new UnityEngine.Vector2(sizeX / 10000f, sizeY / 10000f),
            //             hitboxType = HitboxType.Other
            //         },
            //         TriggerStayId = TriggerStayType.TriggerEvent
            //     }
            // };
            // Fixture fixture = body.CreateFixture(fixtureDef);
            //
            // //2. 启动定时器
            // long timer = postStepTimer.NewFrameTimer(BBTimerInvokeType.TimeFrozeCheckTimer, parser);
            // parser.RegistParam("TimeFroze_Timer", timer);
            // parser.RegistParam("TimeFroze_LastFrame", lastFrame);
            // parser.RegistParam("TimeFroze_CheckBox", fixture);
            // parser.RegistParam("TimeFroze_Hertz", hertz);
            //
            // token.Add(() =>
            // {
            //     postStepTimer.Remove(ref timer);
            //     if (parser.ContainParam("TimeFroze_CheckBox"))
            //     {
            //         Fixture _fixture = parser.GetParam<Fixture>("TimeFroze_CheckBox");
            //         body.DestroyFixture(_fixture);
            //     }
            // });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}