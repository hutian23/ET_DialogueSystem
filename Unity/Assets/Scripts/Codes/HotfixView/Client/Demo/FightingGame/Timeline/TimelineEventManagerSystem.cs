namespace ET.Client
{
    [FriendOf(typeof (TimelineEventManager))]
    public static class TimelineEventManagerSystem
    {
        public static ScriptParser GetParser(this TimelineEventManager self, string trackName)
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