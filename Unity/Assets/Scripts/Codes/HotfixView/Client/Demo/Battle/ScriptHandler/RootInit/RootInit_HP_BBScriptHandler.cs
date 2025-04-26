using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(HPAbility))]
    public class RootInit_HP_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HP";
        }

        //EnableHP: 10000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "HP: (?<MaxHP>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["MaxHP"].Value, out int MaxHP))
            {
                Log.Error($"cannot format {match.Groups["MaxHP"].Value} to int!!!");
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            BuffManager buffManager = unit.GetComponent<BuffManager>();
            HPAbility ability = buffManager.GetComponent<HPAbility>();
            ability.MaxHP = MaxHP;
            ability.CurrentHP = MaxHP;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}