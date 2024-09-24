using System.Text.RegularExpressions;

namespace ET.Client
{
    public class InvokeCallback_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "InvokeCallback";
        }

        //调用回调
        //InvokeCallback type = OnBlock;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            ObjectWait objectWait = parser.GetParent<DialogueComponent>().GetComponent<ObjectWait>();
            Match match = Regex.Match(data.opLine, @"InvokeCallback type = (?<Type>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            var type = match.Groups["Type"].Value;
            switch (type)
            {
                case "OnBlock":
                    objectWait.Notify(new WaitBlock());
                    break;
                case "OnCounterHit":
                    objectWait.Notify(new WaitCounterHit());
                    break;
                case "OnHit":
                    objectWait.Notify(new WaitHit());
                    break;
            }

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}