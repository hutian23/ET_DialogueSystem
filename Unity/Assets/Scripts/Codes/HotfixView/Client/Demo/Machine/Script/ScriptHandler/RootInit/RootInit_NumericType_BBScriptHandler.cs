using System.Text.RegularExpressions;

namespace ET.Client
{
    public class RootInit_NumericType_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "NumericType";
        }

        //NumericType: MaxFall, 300;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "NumericType: (?<NumericType>.*?), (?<Value>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            if (!long.TryParse(match.Groups["Value"].Value, out long value))
            {
                Log.Error($"cannot format {match.Groups["Value"].Value} to long!!!");
                return Status.Failed;
            }
            
            BBNumeric numeric = parser.GetParent<Unit>().GetComponent<BBNumeric>();
            numeric.Set(match.Groups["NumericType"].Value, value, true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}