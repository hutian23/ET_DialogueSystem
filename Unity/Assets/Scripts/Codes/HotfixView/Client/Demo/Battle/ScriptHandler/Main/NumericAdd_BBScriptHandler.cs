using System.Text.RegularExpressions;

namespace ET.Client
{
    public class NumericAdd_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "NumericAdd";
        }

        //NumericAdd: DashCount, -1;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "NumericAdd: (?<NumericType>.*?), (?<Count>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["Count"].Value, out long count))
            {
                Log.Error($"cannot format {match.Groups["Count"].Value} to long");
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            BBNumeric numeric = unit.GetComponent<BBNumeric>();
            
            long oldValue = numeric.GetAsLong(match.Groups["NumericType"].Value);
            numeric.Set(match.Groups["NumericType"].Value, oldValue + count);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}