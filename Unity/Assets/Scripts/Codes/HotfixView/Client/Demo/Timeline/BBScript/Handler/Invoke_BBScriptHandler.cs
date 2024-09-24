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

        //Invoke: 'OnBlock'; 调用一个方法
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "Invoke: '(?<Function>.*?)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            Status ret = await parser.Invoke(match.Groups["Function"].Value, parser.cancellationToken);

            return token.IsCancel() || ret == Status.Failed? Status.Failed : Status.Success;
        }
    }
}