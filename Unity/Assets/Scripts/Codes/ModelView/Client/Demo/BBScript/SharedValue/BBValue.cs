namespace ET.Client
{
    public abstract class BBValue
    {
        public abstract void Recycle();
    }

    public class BBValueBase<T>: BBValue where T : struct
    {
        private T _value;

        public T GetValue()
        {
            return _value;
        }

        public void SetValue(T value)
        {
            _value = value;
        }
        
        public static BBValueBase<T> Create(T value)
        {
            BBValueBase<T> valueBase = ObjectPool.Instance.Fetch<BBValueBase<T>>();
            valueBase.SetValue(value);
            return valueBase;
        }

        public override void Recycle()
        {
            _value = default;
            ObjectPool.Instance.Recycle(this);
        }
    }
}