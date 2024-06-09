using Timeline;

namespace ET.Client
{
    public static class PlayableManagerSystem
    {
        private static TimelinePlayer GetTimelinePlayer(this PlayableManager self)
        {
            return self.GetParent<DialogueComponent>()
                    .GetParent<Unit>()
                    .GetComponent<GameObjectComponent>().GameObject
                    .GetComponent<TimelinePlayer>();
        }

        public static RuntimePlayable GetPlayable(this PlayableManager self)
        {
            return self.GetTimelinePlayer().RuntimeimePlayable;
        }

        public static BBTimeline GetCurrentTimeline(this PlayableManager self)
        {
            return self.GetPlayable().Timeline;
        }

        //behaviorOrder ---> timeline
        public static void Init(this PlayableManager self, long skillOrder)
        {
            BBTimeline currentTimeline = self.GetTimelinePlayer().GetByOrder((int)skillOrder);
            self.GetTimelinePlayer().Init(currentTimeline);
        }
    }
}