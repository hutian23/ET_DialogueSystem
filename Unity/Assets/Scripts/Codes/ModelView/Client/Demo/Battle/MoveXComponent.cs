namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class MoveXComponent : Entity, IAwake<float>, IDestroy
    {
        public float MoveX;
        public ETCancellationToken token = new();
    }
}