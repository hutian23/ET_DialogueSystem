﻿namespace ET.Client
{
    [FriendOf(typeof (GatlingCancel))]
    public static class GatlingCancelSystem
    {
        public class GatlingCancelDestroySystem: DestroySystem<GatlingCancel>
        {
            protected override void Destroy(GatlingCancel self)
            {
                self.cancelTags.Clear();
                self.token?.Cancel();
            }
        }

        public static void Init(this GatlingCancel self)
        {
            self.cancelTags.Clear();
            self.token?.Cancel();
            self.token = new ETCancellationToken();
        }

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
    }
}