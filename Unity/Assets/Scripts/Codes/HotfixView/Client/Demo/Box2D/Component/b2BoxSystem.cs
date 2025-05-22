namespace ET.Client
{
    [FriendOf(typeof(b2Box))]
    public static class b2BoxSystem
    {
        [FriendOf(typeof(b2Body))]
        public class b2BoxDestroySystem : DestroySystem<b2Box>
        {
            protected override void Destroy(b2Box self)
            {
            }
        }
    }
}