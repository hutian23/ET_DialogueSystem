using System.Text.RegularExpressions;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    public class Function_AirDashVelocity_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AirDashVelocity";
        }

        //AirDashVelocity: Vel;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"AirDashVelocity: (?<vel>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["vel"].Value, out long vel))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            //1. 
            Unit unit = parser.GetParent<Unit>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            
            //2.
            float radian = body.GetRadian();
            float x = vel * Mathf.Sin(radian) / 10000f;
            float y = vel * Mathf.Cos(radian) / 10000f;
            body.SetVelocity(new Vector2(x, y));
 
            Log.Warning(new Vector2(x, y).Length().ToString());
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}