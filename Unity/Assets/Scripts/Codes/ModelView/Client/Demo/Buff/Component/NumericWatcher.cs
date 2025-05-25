namespace ET.Client
{
    [ChildOf]
    public class NumericWatcher : Entity, IAwake, IDestroy
    {
        public long _instanceId;  // 调用者
        public int functionIndex; // 回调函数

        protected override string ViewName
        {
            get
            {
                return $"{this.GetType().Name} ({this._instanceId})";
            }
        }
    }
}