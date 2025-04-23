namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class LoopComponent : Entity, IAwake, IDestroy
    {
        public int triggerIndex;
        public int startIndex;
        public int curIndex; // 未来需要考虑，如何将帧同步和Loop结合
        public int endIndex;
        public ETCancellationToken token;
    }
}