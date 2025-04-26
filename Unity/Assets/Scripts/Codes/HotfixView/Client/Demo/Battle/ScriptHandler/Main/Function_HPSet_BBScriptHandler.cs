using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_HPSet_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HPSet";
        }

        //HPSet: HP, InvokeEvent?;
        //HPSet: 1000, true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"HPAdd: (?<Count>.*?), (?<IsEvent>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["Count"].Value, out int count))
            {
                Log.Error($"cannot format {match.Groups["Count"].Value} to int!!!");
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            BuffManager buffManager = unit.GetComponent<BuffManager>();
            HPAbility ability = buffManager.GetComponent<HPAbility>();
            if (ability == null)
            {
                Log.Error($"cannot found HPAbility!!!");
                return Status.Failed;
            }

            bool isEvent = match.Groups["IsEvent"].Value.Equals("true");
            ability.SetHP(count, isEvent);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}