using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_OnFire_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "OnFire";
        }

        //OnFire: 3, 9, 10;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "OnFire: (?<interval>.*?), (?<totalFrame>.*?), (?<damage>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            if (!int.TryParse(match.Groups["interval"].Value, out int interval) ||
                !int.TryParse(match.Groups["totalFrame"].Value, out int totalFrame) ||
                !int.TryParse(match.Groups["damage"].Value, out int damage))
            {
                Log.Error($"cannot format {match.Groups["interval"].Value} / {match.Groups["totalFrame"].Value} / {match.Groups["damage"].Value} to int!!!");
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            BuffManager buffManager = unit.GetComponent<BuffManager>();
            buffManager.RemoveComponent<OnFireBuff>();
            buffManager.AddComponent<OnFireBuff, int, int, int>(interval, totalFrame, damage);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}