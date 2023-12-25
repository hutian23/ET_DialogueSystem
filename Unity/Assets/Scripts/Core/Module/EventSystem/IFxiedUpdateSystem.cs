#if !DOTNET
using System;

namespace ET
{

    public interface IFixedUpdate
    {
    }
    
    public interface IFixedUpdateSystem: ISystemType
    {
        void Run(Entity o);
    }
    
    [ObjectSystem]
    public abstract class FixedUpdateSystem<T>: IFixedUpdateSystem where T : Entity, IFixedUpdate
    {
        public Type Type()
        {
            return typeof (T);
        }

        public Type SystemType()
        {
            return typeof (IFixedUpdateSystem);
        }

        public InstanceQueueIndex GetInstanceQueueIndex()
        {
            return InstanceQueueIndex.FixedUpdate;
        }

        public void Run(Entity o)
        {
            this.FixedUpdate((T)o);
        }

        protected abstract void FixedUpdate(T self);
    }
}
#endif