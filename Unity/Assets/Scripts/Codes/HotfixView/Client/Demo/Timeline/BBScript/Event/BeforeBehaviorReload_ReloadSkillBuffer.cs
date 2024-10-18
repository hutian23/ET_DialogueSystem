namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof(SkillBuffer))]
    public class BeforeBehaviorReload_ReloadSkillBuffer : AEvent<BeforeBehaviorReload>
    {
        protected override async ETTask Run(Scene scene, BeforeBehaviorReload args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            SkillBuffer buffer = timelineComponent.GetComponent<SkillBuffer>();

            //1. 重新启动行为机定时器(在加特林取消中，需要更改进入行为的条件)
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            bbTimer.Remove(ref buffer.CheckTimer);
            buffer.CheckTimer = bbTimer.NewFrameTimer(BBTimerInvokeType.BehaviorCheckTimer, buffer);

            InputWait inputWait = timelineComponent.GetComponent<InputWait>();
            inputWait.CancelNotifyTimer();
            
            await ETTask.CompletedTask;
        }
    }
}