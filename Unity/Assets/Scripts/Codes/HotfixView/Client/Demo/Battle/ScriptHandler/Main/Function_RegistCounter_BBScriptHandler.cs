using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_RegistCounter_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistCounter";
        }

        // RegistCounter: 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"RegistCounter: (?<WaitFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["WaitFrame"].Value, out int waitFrame))
            {
                Log.Error($"cannot format {match.Groups["WaitFrame"].Value} to int!");
                return Status.Failed;
            }
            
            parser.RemoveComponent<Counter>();
            parser.AddComponent<Counter, int>(waitFrame);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}