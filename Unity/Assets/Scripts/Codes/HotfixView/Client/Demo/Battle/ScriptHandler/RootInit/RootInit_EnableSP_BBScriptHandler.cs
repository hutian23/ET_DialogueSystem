using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(SPAbility))]
    public class RootInit_EnableSP_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableSP";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "EnableSP: (?<MaxSP>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["MaxSP"].Value, out int MaxSP))
            {
                Log.Error($"cannot format {match.Groups["MaxSP"].Value} to int!!!");
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            BuffManager buffManager = unit.GetComponent<BuffManager>();

            buffManager.RemoveComponent<SPAbility>();
            SPAbility ability = buffManager.AddComponent<SPAbility>();
            ability.MaxSP = MaxSP;
            ability.CurrentSP = MaxSP;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}