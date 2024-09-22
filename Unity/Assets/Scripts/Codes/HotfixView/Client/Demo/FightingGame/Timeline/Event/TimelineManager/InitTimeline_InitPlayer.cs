using System.Linq;
using Timeline;

namespace ET.Client
{
    [Event(SceneType.Current)]
    public class InitTimeline_InitPlayer : AEvent<InitTimeline>
    {
        protected override async ETTask Run(Scene scene, InitTimeline a)
        {
            Unit player = TODUnitHelper.GetPlayer(scene.ClientScene());
            TimelineComponent timelineComponent = player.GetComponent<TimelineComponent>();
            TimelinePlayer timelinePlayer = timelineComponent.GetTimelinePlayer();

            BBTimeline Idle = timelinePlayer.BBPlayable.GetTimelines().FirstOrDefault();
            timelinePlayer.Init(Idle);
            
            await ETTask.CompletedTask;
        }
    }
}