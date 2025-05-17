using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_BulletAccelY_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BulletAccelY";
        }

        //BulletAccelY: 
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "BulletAccelY: (?<startV>.*?), (?<lastFrame>.*?), (?<accel>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["startV"].Value, out long startV) ||
                !int.TryParse(match.Groups["lastFrame"].Value, out int lastFrame) ||
                !long.TryParse(match.Groups["accel"].Value, out long accel))
            {
                Log.Error($"match failed");
                return Status.Failed;
            }

            long instanceId = parser.GetParam<long>("CreateBullet_UnitId");
            Unit unitB = Root.Instance.Get(instanceId) as Unit;
            BBParser parserB = unitB.GetComponent<BBParser>();
            
            parserB.RemoveComponent<AccelYComponent>(); 
            parserB.AddComponent<AccelYComponent, float, float, int>(startV / 10000f, accel / 10000f, lastFrame, true);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}