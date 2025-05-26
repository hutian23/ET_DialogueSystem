namespace ET.Client
{
    [ChildOf]
    public class NumericWatcher : Entity, IAwake<long, int, string>, IDestroy
    {
        public long _instanceId;  // 调用者
        public int functionIndex; // 回调函数
        public string watcherName;
    }

    public struct NumericWatcherCallback
    {
        public long instanceId;
    }
}