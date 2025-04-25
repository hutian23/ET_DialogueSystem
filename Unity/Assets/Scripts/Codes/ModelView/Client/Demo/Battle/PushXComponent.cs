namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class PushXComponent : Entity, IAwake<int, float, float>, IDestroy
    {
        public int direction;
        public float startVel;
        public float curVel;
        public float friction;
        public ETCancellationToken token;
    }
}