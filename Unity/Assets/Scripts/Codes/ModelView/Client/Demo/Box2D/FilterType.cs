namespace ET.Client
{
    public static class FilterType
    {
        public const int None = 0;
        public const int PushBoxFilter = 1;
    }

    public struct B2FilterCallback
    {
        public long instanceId; // b2Filter.instanceId
        public long instanceIdA;
        public long instanceIdB;
    }
}