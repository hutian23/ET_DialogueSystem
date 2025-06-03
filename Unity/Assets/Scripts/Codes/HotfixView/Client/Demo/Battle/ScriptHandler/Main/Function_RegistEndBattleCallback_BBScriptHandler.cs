using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_RegistEndBattleCallback_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistEndBattleCallback";
        }

        //RegistEndBattleCallback: GroupName, FunctionName;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"RegistEndBattleCallback: (?<GroupName>\w+), (?<FunctionName>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            BuffManager buffManager = parser.GetParent<Unit>().GetComponent<BuffManager>();
            buffManager.RemoveComponent<EndBattleWatcher>();
            buffManager.AddComponent<EndBattleWatcher, string, string>(match.Groups["GroupName"].Value, match.Groups["FunctionName"].Value, true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}