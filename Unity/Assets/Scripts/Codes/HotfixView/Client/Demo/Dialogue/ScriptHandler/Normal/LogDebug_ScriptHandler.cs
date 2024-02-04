using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Log_ScriptHandler : ScriptHandler
    {
        public override string GetOPType()
        {
            return "Log";
        }

        //Log.Debug("22323");
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"Log\.Debug\(""(.*?)""\);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }
            Log.Debug(match.Groups[1].Value);
            await ETTask.CompletedTask;
        }
    }
}