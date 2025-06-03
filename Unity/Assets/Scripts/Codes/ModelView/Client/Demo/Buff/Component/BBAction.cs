namespace ET.Client
{
    [ChildOf]
    public class BBAction : Entity, IAwake<long, int>, IDestroy
    {
        public long _instanceId;  // 调用者
        public int functionIndex; // 回调函数
    }

    public struct BBActionCallback
    {
        public long instanceId;
    }
}