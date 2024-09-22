using Timeline;

namespace ET.Client
{
    public static class PlayableManagerSystem
    {
        private static TimelinePlayer GetTimelinePlayer(this TimelineManager self)
        {
            return self.GetParent<DialogueComponent>()
                    .GetParent<Unit>()
                    .GetComponent<GameObjectComponent>().GameObject
                    .GetComponent<TimelinePlayer>();
        }

        public static RuntimePlayable GetPlayable(this TimelineManager self)
        {
            return self.GetTimelinePlayer().RuntimeimePlayable;
        }

        public static BBTimeline GetCurrentTimeline(this TimelineManager self)
        {
            return self.GetPlayable().Timeline;
        }

        //behaviorOrder ---> timeline
        public static void Init(this TimelineManager self, long skillOrder)
        {
            BBTimeline currentTimeline = self.GetTimelinePlayer().GetByOrder((int)skillOrder);
            self.GetTimelinePlayer().Init(currentTimeline);
        }
    }
}