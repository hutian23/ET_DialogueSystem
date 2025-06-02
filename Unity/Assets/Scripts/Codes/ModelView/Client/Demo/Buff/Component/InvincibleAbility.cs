namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class InvincibleAbility : Entity, IAwake<int, long>, IDestroy, IFrameUpdate
    {
        public int waitFrame;
        public int cnt;
        public long unitId;
    }
}