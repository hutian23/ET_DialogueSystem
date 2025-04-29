namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class OnFireBuff : Entity, IAwake<int, int, int>, IDestroy
    {
        public int interval;
        public int totalFrame;
        public int curFrame;
        public int damage;
        public ETCancellationToken token;
    }
}