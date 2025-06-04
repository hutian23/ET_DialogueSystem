namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class JumpMoveXComponent : Entity, IAwake<float>, IDestroy
    {
        public float vel;
        public ETCancellationToken token;
    }
}