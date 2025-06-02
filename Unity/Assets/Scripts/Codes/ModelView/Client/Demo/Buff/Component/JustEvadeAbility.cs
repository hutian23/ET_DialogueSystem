namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class JustEvadeAbility : Entity, IAwake<int>, IFrameUpdate, IDestroy
    {
        public bool canJustEvade;
        public int chargeFrame;
        public int cnt;
    }
}