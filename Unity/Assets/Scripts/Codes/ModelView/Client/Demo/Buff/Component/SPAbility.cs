namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class SPAbility : Entity, IAwake, IDestroy
    {
        public int MaxSP;
        public int CurrentSP;
    }

    public struct SpChangeCallback
    {
        public long instanceId;
        public int preSP;
        public int curSP;
    }
}