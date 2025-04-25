namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class AirMoveXComponent : Entity, IAwake<float>, IDestroy
    {
        public float vel;
        public ETCancellationToken token;
    }
}