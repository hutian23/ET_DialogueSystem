using Testbed.Abstractions;

namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof (BBTimerComponent))]
    public class UpdateTimelineComponent_ManageInputTimer: AEvent<UpdateTimelineComponent>
    {
        protected override async ETTask Run(Scene scene, UpdateTimelineComponent args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            
            //不是玩家unit
            InputWait inputWait = timelineComponent.GetComponent<InputWait>();
            if (inputWait == null) return;

            //这个timer用于控制输入
            BBTimerComponent inputTimer = timelineComponent.GetComponent<InputWait>().GetComponent<BBTimerComponent>();
            //Pause state
            if (Global.Settings.Pause && inputTimer._gameTimer.IsRunning)
            {
                inputTimer.Pause();
            }

            //Running state
            if (!Global.Settings.Pause && !inputTimer._gameTimer.IsRunning)
            {
                inputTimer.Restart();
            }

            //Update one step
            if (Global.Settings.SingleStep)
            {
                inputTimer.Accumulator += inputTimer.GetFrameLength();
            }

            await ETTask.CompletedTask;
        }
    }
}