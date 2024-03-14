namespace ET.Client
{
    [FriendOf(typeof(WhiffCancel))]
    public static class WhiffCancelSystem
    {
        public class WhiffCancelDestroySystem : DestroySystem<WhiffCancel>
        {
            protected override void Destroy(WhiffCancel self)
            {
            }
        }

        public static void Init(this WhiffCancel self)
        {
            self.cancelTags.Clear();
            self.token?.Cancel();
            self.token = new ETCancellationToken();
        }

        public static void AddTag(this WhiffCancel self, string tag)
        {
            self.cancelTags.Add(tag);
        }

        public static bool ContainTag(this WhiffCancel self, string tag)
        {
            return self.cancelTags.Contains(tag);
        }

        public static void RemoveTag(this WhiffCancel self, string tag)
        {
            self.cancelTags.Remove(tag);
        }
    }
}