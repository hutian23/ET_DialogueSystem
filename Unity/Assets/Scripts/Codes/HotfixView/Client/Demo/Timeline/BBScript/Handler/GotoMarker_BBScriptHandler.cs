using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    public class GotoMarker_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GotoMarker";
        }

        //GotoMarker: 'Loop';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "GotoMarker: '(?<marker>.*?)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            parser.function_Pointers[data.functionID] = parser.GetMarker(match.Groups["marker"].Value);
            await TimerComponent.Instance.WaitFrameAsync(token);
            return token.IsCancel()? Status.Failed : Status.Success;
        }
    }
}