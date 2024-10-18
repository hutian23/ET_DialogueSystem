namespace ET.Client
{
    [FriendOf(typeof (CancelManager))]
    public class CloseCancelWindow_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CloseCancelWindow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            InputWait inputWait = parser.GetParent<TimelineComponent>().GetComponent<InputWait>();
            inputWait.SetOpenWindow(false);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}