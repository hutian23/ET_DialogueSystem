namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class HPAbility : Entity, IAwake, IDestroy
    {
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