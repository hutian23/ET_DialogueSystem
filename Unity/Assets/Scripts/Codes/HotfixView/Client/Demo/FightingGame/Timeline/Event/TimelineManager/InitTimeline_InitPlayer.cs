using Timeline;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class InitTimeline_InitPlayer : AEvent<InitTimeline>
    {
        protected override async ETTask Run(Scene scene, InitTimeline args)
        {
            Unit player = TODUnitHelper.GetPlayer(scene.ClientScene());
            TimelineComponent timelineComponent = player.GetComponent<TimelineComponent>();
            await ETTask.CompletedTask;
        }
    }
}