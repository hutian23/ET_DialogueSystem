namespace ET.Client
{
    [FriendOf(typeof (SkillInfo))]
    [FriendOf(typeof (CancelManager))]
    public class OpenCancelWindow_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "OpenCancelWindow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            InputWait inputWait = parser.GetParent<TimelineComponent>().GetComponent<InputWait>();
            inputWait.SetOpenWindow(true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}