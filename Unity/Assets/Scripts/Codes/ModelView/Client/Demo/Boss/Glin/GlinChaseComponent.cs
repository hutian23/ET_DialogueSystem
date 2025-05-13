namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class GlinChaseComponent : Entity, IAwake, IDestroy, IGizmosUpdate
    {
        public float minRotate;
        public float maxRotate;
        public float curRotate;
        public float damping;
        public ETCancellationToken token;
    }
}