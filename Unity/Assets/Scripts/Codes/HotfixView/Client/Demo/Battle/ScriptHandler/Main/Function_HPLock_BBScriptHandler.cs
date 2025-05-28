using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_HPLock_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HPLock";
        }

        //HPLock: MinHP;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"HPLock: (?<Count>.*?);");
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
            
            ability.SetMinHP(count);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}