namespace ET.Client
{
    [FriendOf(typeof(SkillBuffer))]
    public class DisposeGCWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DisposeGCWindow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();

            //1. 启动输入窗口
            InputWait inputWait = timelineComponent.GetComponent<InputWait>();
            inputWait.CancelNotifyTimer();

            //2. 退出加特林取消的行为控制器
            SkillBuffer buffer = timelineComponent.GetComponent<SkillBuffer>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            bbTimer.Remove(ref buffer.CheckTimer);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}