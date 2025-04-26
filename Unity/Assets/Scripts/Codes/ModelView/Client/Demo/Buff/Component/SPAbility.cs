namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class SPAbility : Entity, IAwake<int>, IDestroy
    {
        public int MaxSP;
        public int CurrentSP;
    }

    public struct SPChangeCallback
    {
        public long instanceId;
        public int preSP;
        public int curSP;
    }
}