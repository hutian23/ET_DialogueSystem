using System.Text.RegularExpressions;
using Sirenix.Utilities.Editor;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    public class If_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "If";
        }

        //if HP > 10:
        //  LogWarning: 'Hello world';
        //  LogWarning: '111';
        //  if HP < 10:
        //      LogWarning: '222';
        //  LogWarning: '222';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "if (?<Check>.*?):");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //获取条件码 HP > 10
            Match match2 = Regex.Match(match.Groups["Check"].Value, @"^\w+");
            if (!match2.Success)
            {
                Log.Error($"not found trigger: {data.opLine}");
                return Status.Failed;
            }

            string triggerType = match2.Value;
            BBTriggerHandler handler = DialogueDispatcherComponent.Instance.GetTrigger(triggerType);
            bool ret = handler.Check(parser, data);

            if (ret)
            {
                var opLines = parser.opLines.Split('\n');
                while (parser.function_Pointers[data.functionID] < opLines.Length)
                {
                    string opLine = opLines[++parser.function_Pointers[data.functionID]];
                    if (string.IsNullOrEmpty(opLine)) continue;

                    if (!opLine.StartsWith('\t'))
                    {
                        Log.Error($"格式错误!:{opLine}");
                        return Status.Failed;
                    }

                    //去除空行
                    opLine = opLine.Trim();
                    if (string.IsNullOrEmpty(opLine) || opLine.StartsWith('#')) continue;
                    Match match3 = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                }
            }

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}