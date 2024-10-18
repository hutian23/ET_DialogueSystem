namespace ET.Client
{
    [FriendOf(typeof(SkillBuffer))]
    public class OpenGCWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "OpenGCWindow";
        }

        //OpenGCWindow;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();

            //1. 启动输入窗口
            InputWait inputWait = timelineComponent.GetComponent<InputWait>();
            inputWait.StartNotifyTimer();

            //2. 启动加特林窗口
            SkillBuffer buffer = timelineComponent.GetComponent<SkillBuffer>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            bbTimer.Remove(ref buffer.CheckTimer);
            buffer.CheckTimer = bbTimer.NewFrameTimer(BBTimerInvokeType.GatlingCancelCheckTimer, buffer);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}