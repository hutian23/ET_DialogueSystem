using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (DialogueComponent))]
    public class BehaviorBufferComponent: Entity, IAwake, IDestroy
    {
        public Queue<BehaviorBuffer> workQueue = new();
    }

    public class BehaviorBuffer
    {
        public long order;

        public static BehaviorBuffer Create(long order)
        {
            BehaviorBuffer buffer = ObjectPool.Instance.Fetch<BehaviorBuffer>();
            buffer.order = order;
            return buffer;
        }

        public void Recycle()
        {
            this.order = 0;
            ObjectPool.Instance.Recycle(this);
        }
    }
}