using System.Numerics;
using System.Text.RegularExpressions;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    public class RootInit_EnableAirCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableAirCheck";
        }

        //EnableAirCheck: true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"EnableAirCheck: (?<CenterX>-?\d+), (?<CenterY>-?\d+), (?<SizeX>-?\d+), (?<SizeY>-?\d+);");
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
            
            //2. 初始化
            Unit unit = parser.GetParent<Unit>();
            BuffManager buffManager = unit.GetComponent<BuffManager>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            
            buffManager.RemoveComponent<AirCheckAbility>();
            body.ClearFixtures(FixtureType.AirCheckBox); //移除夹具
            
            //3. 创建夹具
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
                    Type = FixtureType.AirCheckBox,
                    LayerMask = LayerType.Unit,
                    IsTrigger = true,
                    UserData = new BoxInfo()
                    {
                        boxName = "AirCheckBox",
                        center = new UnityEngine.Vector2(centerX, centerY) / 1000f,
                        size = new UnityEngine.Vector2(sizeX, sizeY) / 1000f,
                        hitboxType = HitboxType.Other
                    },
                    TriggerStayId = TriggerStayType.TriggerEvent
                }
            };
            body.CreateFixture(fixtureDef);
            
            //4. 添加组件
            buffManager.AddComponent<AirCheckAbility>();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}