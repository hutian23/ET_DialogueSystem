using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (DialogueComponent))]
    public class BehaviorBufferComponent: Entity, IAwake, IDestroy
    {
        public long bufferCheckTimer;
        public Queue<BehaviorBuffer> BufferQueue = new();
        public HashSet<long> OrderSet = new();
    }

    public class BehaviorBuffer
    {
        public long order;
        public long startFrame;
        public long LastedFrame;

        public static BehaviorBuffer Create(long order, long startFrame, long lastedFrame)
        {
            BehaviorBuffer buffer = ObjectPool.Instance.Fetch<BehaviorBuffer>();
            buffer.order = order;
            buffer.startFrame = startFrame;
            buffer.LastedFrame = lastedFrame;
            return buffer;
        }

        public void Recycle()
        {
            this.order = 0;
            this.startFrame = 0;
            this.LastedFrame = 0;
            ObjectPool.Instance.Recycle(this);
        }
    }
}