using Testbed.Abstractions;

namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof (BBTimerComponent))]
    public class UpdateTimelineComponent_ManageCombatTimer: AEvent<UpdateTimelineComponent>
    {
        protected override async ETTask Run(Scene scene, UpdateTimelineComponent args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            //Pause state
            if (Global.Settings.Pause && bbTimer._gameTimer.IsRunning)
            {
                bbTimer.Pause();
            }

            //Running state
            if (!Global.Settings.Pause && !bbTimer._gameTimer.IsRunning)
            {
                bbTimer.Restart();
            }

            //update one step
            if (Global.Settings.SingleStep)
            {
                bbTimer.Accumulator += bbTimer.GetFrameLength();
            }
            await ETTask.CompletedTask;
        }
    }
}