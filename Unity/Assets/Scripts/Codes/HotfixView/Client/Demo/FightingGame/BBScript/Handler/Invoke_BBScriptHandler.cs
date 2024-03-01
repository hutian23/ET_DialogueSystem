using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    public class Invoke_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Invoke";
        }

        //Invoke func = OnBlock; 调用一个方法
        public override async ETTask<Status> Handle(Unit unit, string opCode, ETCancellationToken token)
        {
            Match match = Regex.Match(opCode, @"Invoke func = (?<Function>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(opCode);
                return Status.Failed;
            }

            BBParser bbParser = unit.GetComponent<DialogueComponent>().GetComponent<BBParser>();
            Status ret = await bbParser.Invoke(match.Groups["Function"].Value, token);
            
            return token.IsCancel() || ret == Status.Failed? Status.Failed : Status.Success;
        }
    }
}