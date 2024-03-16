using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (DialogueComponent))]
    public class BehaviorBufferComponent: Entity, IAwake, IDestroy
    {
        public long bufferTimer;
        public Queue<BehaviorBuffer> BufferQueue = new();
        public List<long> OrderList = new();

        //技能优先级和节点ID的映射
        public Dictionary<long, uint> targetIDDict = new();
        public Dictionary<uint, BehaviorInfo> behaviorDict = new();
        //协程化输入检测
        public Dictionary<uint, InputCheck> inputCheckDict = new();
        //每帧检测
        public Dictionary<uint, TriggerCheck> triggerCheckDict = new();
    }

    public class BehaviorBuffer
    {
        public long order;
        public long startFrame;
        public long LastedFrame;
        public uint targetID;
        
        public static BehaviorBuffer Create(long order, long startFrame, long lastedFrame,uint targetID)
        {
            BehaviorBuffer buffer = ObjectPool.Instance.Fetch<BehaviorBuffer>();
            buffer.order = order;
            buffer.startFrame = startFrame;
            buffer.LastedFrame = lastedFrame;
            buffer.targetID = targetID;
            return buffer;
        }

        public void Recycle()
        {
            this.order = 0;
            this.startFrame = 0;
            this.LastedFrame = 0;
            this.targetID = 0;
            ObjectPool.Instance.Recycle(this);
        }
    }
}