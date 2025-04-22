namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class LoopComponent : Entity, IAwake, IDestroy
    {
        public int triggerIndex;
        public int startIndex;
        public int endIndex;
        public ETCancellationToken token;
    }
}