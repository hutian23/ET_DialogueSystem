namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class RepeatedTimerCallback : Entity, IAwake<int, int>, IDestroy
    {
        public int waitFrame;
        public int functionIndex;
        public ETCancellationToken token;
    }
}