using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(WhiffCancelComponent))]
    public class WhiffOption_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "WhiffOption";
        }

        //WhiffOption: Rg_Jump;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"WhiffOption: (?<Option>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            WhiffCancelComponent whiff = parser.GetComponent<WhiffCancelComponent>();
            whiff.Options.Add(match.Groups["Option"].Value);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}