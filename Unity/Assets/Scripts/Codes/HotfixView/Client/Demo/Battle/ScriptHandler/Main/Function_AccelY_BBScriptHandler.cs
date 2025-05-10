using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(AccelYComponent))]
    public class Function_AccelY_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AccelY";
        }

        //AccelY: 0, 100000, 50000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "AccelY: (?<startV>.*?), (?<lastFrame>.*?), (?<accel>.*?);");
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

            parser.RemoveComponent<AccelYComponent>(); 
            parser.AddComponent<AccelYComponent, float, float, int>(startV / 10000f, accel / 10000f, lastFrame, true);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}