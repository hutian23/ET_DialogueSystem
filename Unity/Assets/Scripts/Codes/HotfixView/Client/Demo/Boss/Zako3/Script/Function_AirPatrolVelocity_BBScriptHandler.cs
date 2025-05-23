using System;
using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_AirPatrolVelocity_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AirPatrolVelocity";
        }

        //AirPatrolVelocity: Velocity;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"AirPatrolVelocity: (?<Velocity>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["Velocity"].Value, out long velocity))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);

            //1. 速度方向
            Vector2 direction = UnityEngine.Random.insideUnitCircle.ToVector2();
            direction.X = Math.Abs(direction.X); // 限定横向速度，朝向前方
            
            //2. 速度大小
            body.SetVelocity(direction * velocity / 10000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}