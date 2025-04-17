using System;

namespace ET.Client
{
    public abstract class BBValue
    {
        public abstract Type ValueType { get; }
    }

    public interface BBValue<T> where T : struct
    {
        public T GetValue();
        public void SetValue(T value);

        public BBValue<T> Create(T value);
        
        public void Destroy();
    }

    public abstract class BBValueBase<T>: BBValue, BBValue<T> where T : struct
    {
        protected T Value;
        
        public T GetValue()
        {
            return Value;
        }

        public void SetValue(T value)
        {
            Value = value;
        }

        public BBValue<T> Create(T value)
        {
            BBValueBase<T> valueBase = ObjectPool.Instance.Fetch<BBValueBase<T>>();
            valueBase.SetValue(value);
            return valueBase;
        }

        public void Destroy()
        {
            Value = default;
            ObjectPool.Instance.Recycle(this);
        }
    }
}