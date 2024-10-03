namespace ET.Client
{
    [UniqueId(0, 1000)]
    public static class BBTimerInvokeType
    {
        public const int None = 0;
        public const int Test1 = 1;
        public const int CheckInput = 2;

        public const int BehaviorBuffer_TriggerTimer = 3;
        public const int BehaviorBufferCheckTimer = 4;
        public const int BehaviorTimer = 5;
        public const int SkillCheckTimer = 6;
    }
}