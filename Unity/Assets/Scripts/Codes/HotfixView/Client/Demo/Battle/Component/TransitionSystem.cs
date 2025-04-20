namespace ET.Client
{
    [FriendOf(typeof(Transition))]
    public static class TransitionSystem
    {
        public class TransitionDestroySystem : DestroySystem<Transition>
        {
            protected override void Destroy(Transition self)
            {
                self.Flags.Clear();
                self.cachedFlags.Clear();
            }
        }

        public static bool CheckFlag(this Transition self, string flag)
        {
            return self.Flags.Contains(flag);
        }

        public static bool CheckCachedFlag(this Transition self, string flag)
        {
            return self.cachedFlags.Contains(flag);
        }

        public static void CacheFlag(this Transition self)
        {
            self.cachedFlags.Clear();
            foreach (string flag in self.Flags)
            {
                self.cachedFlags.Add(flag);
            }
        }
    }
}