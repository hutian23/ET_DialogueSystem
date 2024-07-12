using Timeline;

namespace ET.Client
{
    [FriendOf(typeof (TimelineEventManager))]
    public static class TimelineEventManagerSystem
    {
        public static async ETTask<Status> Evaluate(this TimelineEventManager self, int targetFrame)
        {
            TimelinePlayer timelinePlayer = self.GetParent<TimelineComponent>().GetTimelinePlayer();

            foreach (RuntimeTrack runtimeTrack in timelinePlayer.RuntimeimePlayable.RuntimeTracks)
            {
                if (runtimeTrack is not RuntimeEventTrack runtimeEventTrack)
                {
                    continue;
                }
                
                //find event of targetFrame
                BBEventTrack track = runtimeEventTrack.Track as BBEventTrack;
                EventInfo info = track.GetInfo(targetFrame);

                //Handle Event
                ScriptParser parser = self.GetParser(track.Name);
                // parser.InitScript(info.);
            }

            await ETTask.CompletedTask;
            return Status.Success;
        }

        private static ScriptParser GetParser(this TimelineEventManager self, string trackName)
        {
            if (!self.parserDict.TryGetValue(trackName, out long id))
            {
                Log.Error($"not found track: {trackName}");
                return null;
            }

            ScriptParser parser = self.GetChild<ScriptParser>(id);
            if (parser == null)
            {
                Log.Error($"not found scriptparser of eventtrack:{trackName}");
                return null;
            }

            return self.GetChild<ScriptParser>(id);
        }
    }
}