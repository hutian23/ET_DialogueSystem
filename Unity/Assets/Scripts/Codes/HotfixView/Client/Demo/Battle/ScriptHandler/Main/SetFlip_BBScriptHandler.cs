using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    public class SetFlip_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetFlip";
        }

        // SetFlip: Left;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SetFlip: (?<Flip>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            b2Body body = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);
            FlipState flip = match.Groups["Flip"].Value.Equals("Left")? FlipState.Left : FlipState.Right;
            body.SetFlip(flip);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}