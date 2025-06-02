namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class JustEvadeAbility : Entity, IAwake, IFrameUpdate, IDestroy
    {
        public bool canJustEvade;
        public int chargeFrame;
        public int cnt;
    }
}