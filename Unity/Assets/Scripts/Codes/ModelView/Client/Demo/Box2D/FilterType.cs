namespace ET.Client
{
    public static class FilterType
    {
        public const int None = 0;
        public const int PushBoxFilter = 1;
        public const int BulletHitFilter = 2;
        public const int InvincibleFilter = 3;
        public const int InvalidHitboxFilter = 4;
    }

    public struct B2FilterCallback
    {
        public long instanceIdA;
        public long instanceIdB;
    }
}