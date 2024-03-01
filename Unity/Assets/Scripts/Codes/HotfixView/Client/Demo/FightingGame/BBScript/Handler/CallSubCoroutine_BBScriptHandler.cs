using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CallSubCoroutine_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CallSubCoroutine";
        }

        //CallSubCoroutine func = OnBlock;
        public override async ETTask<Status> Handle(Unit unit, string opCode, ETCancellationToken token)
        {
            Match match = Regex.Match(opCode, @"CallSubCoroutine func = (?<Function>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(opCode);
                return Status.Failed;
            }

            unit.GetComponent<DialogueComponent>().GetComponent<BBParser>().SubCoroutine(match.Groups["Function"].Value).Coroutine();
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}