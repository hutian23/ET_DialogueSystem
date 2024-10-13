namespace ET.Client
{
    public class CloseInputWindow_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CloseInputWindow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            InputWait inputWait = parser.GetParent<TimelineComponent>().GetComponent<InputWait>();
            inputWait.CancelTimer();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}