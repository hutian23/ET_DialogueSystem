using System.Text.RegularExpressions;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Timeline;
using UnityEngine;

namespace ET.Client
{
    public class VC_Sensor_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Sensor";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"VC_Sensor: (?<CenterX>.*?), (?<CenterY>.*?), (?<SizeX>.*?), (?<SizeY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["CenterX"].Value, out long centerX) || !long.TryParse(match.Groups["CenterY"].Value, out long centerY) ||
                !long.TryParse(match.Groups["SizeX"].Value, out long sizeX) || !long.TryParse(match.Groups["SizeY"].Value, out long sizeY))
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            // 跟随对象的实体的刚体创建夹具
            b2Body body = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);
            PolygonShape shape = new();
            shape.SetAsBox(sizeX / 20000f, sizeY / 20000f, new System.Numerics.Vector2(centerX, centerY) / 10000f, 0f);
            FixtureDef fixtureDef = new()
            {
                Shape = shape,
                Density = 0f,
                Friction = 0f,
                UserData = new FixtureData()
                {
                    InstanceId = body.InstanceId,
                    Name = "VC_Sensor",
                    Type = FixtureType.Default,
                    LayerMask = LayerType.Camera,
                    IsTrigger = true,
                    UserData = new BoxInfo()
                    {
                        boxName = "VC_Sensor",
                        center = new Vector2(centerX, centerY) / 10000f,
                        size = new Vector2(sizeX, sizeY) / 10000f,
                        hitboxType = HitboxType.Other
                    }
                }
            };
            body.CreateFixture(fixtureDef);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}