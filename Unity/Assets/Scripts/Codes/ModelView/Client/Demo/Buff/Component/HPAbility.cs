namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class HPAbility : Entity, IAwake<int>, IDestroy
    {
        public int MinHP; // 锁血，当前血量不能小于这个值
        public int MaxHP;
        public int CurrentHP;
    }

    public struct HPChangeCallback
    {
        public long instanceId;
        public int preHP;
        public int curHP;
    }
}