using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (BBInputComponent))]
    public class BBBehaviorBufferComponent: Entity, IAwake, IDestroy, IUpdate
    {
        //这一帧过期了,回收所有的BehaviorBuffer
        public readonly Queue<BehaviorBuffer> BufferQueue = new(100);
        public readonly SortedDictionary<long, BehaviorBuffer> workDict = new();
        public long idGenerator;
        public long timer;
    }

    public struct WaitNextSkill : IWaitType
    {
        public uint targetID;
        public int Error { get; set; }
    }
    
    //缓冲行为
    public class BehaviorBuffer
    {
        public long Id;
        public uint targetID;
        public long skillOrder;
        public long startFrame;
        public long LastedFrame;
        public List<string> triggers = new();

        public static BehaviorBuffer Create(long Id, uint targetID, long skillOrder, long startFrame, long LastedFrame, List<string> _triggers)
        {
            BehaviorBuffer buffer = ObjectPool.Instance.Fetch<BehaviorBuffer>();
            buffer.targetID = targetID;
            buffer.Id = Id;
            buffer.skillOrder = skillOrder;
            buffer.startFrame = startFrame;
            buffer.LastedFrame = LastedFrame;
            _triggers.ForEach(t => { buffer.triggers.Add(t); });
            return buffer;
        }

        public void Recycle()
        {
            this.Id = 0;
            this.targetID = 0;
            this.skillOrder = 0;
            this.startFrame = 0;
            this.LastedFrame = 0;
            this.triggers.Clear();
        }
    }
}