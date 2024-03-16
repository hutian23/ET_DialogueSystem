namespace ET.Client
{
    [FriendOf(typeof (BehaviorInfo))]
    public static class BehaviorInfoSystem
    {
        public static long GetOrder(this BehaviorInfo self)
        {
            ulong result = 0;
            result |= self.order;
            result |= (ulong)self.skillType << 16;
            return (long)result;
        }
    }
}