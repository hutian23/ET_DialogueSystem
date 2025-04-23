namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class AirMoveXComponent : Entity, IAwake<float>, IDestroy
    {
        public float vel;
        public bool inertiaEffect;
        public ETCancellationToken token;
    }
}