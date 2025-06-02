using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_TacticalTime_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "TacticalTime";
        }

        // 子弹时间 TacticalTime: LastFrame, Hertz;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "TacticalTime: (?<lastFrame>.*?), (?<hertz>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["lastFrame"].Value, out int lastFrame) ||
                !int.TryParse(match.Groups["hertz"].Value, out int hertz))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }

            BuffManager buffManager = parser.GetParent<Unit>().GetComponent<BuffManager>();
            buffManager.RemoveComponent<TacticalTimeBuff>();
            buffManager.AddComponent<TacticalTimeBuff, int, int>(lastFrame, hertz, true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}