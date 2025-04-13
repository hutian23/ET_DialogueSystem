namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class Counter : Entity, IAwake<int>, IDestroy
    {
        public int totalFrame;
        public int curFrame;
        public ETCancellationToken Token;
    }
}