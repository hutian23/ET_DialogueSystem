namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class InvincibleAbility : Entity, IAwake<int>, IDestroy
    {
        public int waitFrame;
        public ETCancellationToken token;
    }
}