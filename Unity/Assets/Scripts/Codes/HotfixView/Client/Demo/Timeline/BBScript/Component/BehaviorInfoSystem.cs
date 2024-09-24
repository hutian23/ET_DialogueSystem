namespace ET.Client
{
    [FriendOf(typeof (BehaviorInfo))]
    public static class BehaviorInfoSystem
    {
        public static long GetOrder(this BehaviorInfo self)
        {
            return self.order; 
        }
    }
}