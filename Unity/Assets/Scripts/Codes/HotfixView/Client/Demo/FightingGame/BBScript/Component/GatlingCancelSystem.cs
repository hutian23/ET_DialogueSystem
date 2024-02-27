namespace ET.Client
{
    [FriendOf(typeof(GatlingCancel))]
    public static class GatlingCancelSystem
    {
        public static void AddTag(this GatlingCancel self, string tag)
        {
            self.cancelTags.Add(tag);
        }

        public static bool ContainTag(this GatlingCancel self, string tag)
        {
            return self.cancelTags.Contains(tag);
        }

        public static void RemoveTag(this GatlingCancel self, string tag)
        {
            self.cancelTags.Remove(tag);
        }
        
        public static void Clear(this GatlingCancel self)
        {
            self.cancelTags.Clear();
        }
    }
}