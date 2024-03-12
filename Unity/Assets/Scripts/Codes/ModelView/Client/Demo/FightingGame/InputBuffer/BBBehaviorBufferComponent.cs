using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (BBInputComponent))]
    public class BBBehaviorBufferComponent: Entity, IAwake, IDestroy, IUpdate
    {
        public readonly MultiMap<long, long> bufferIds = new();

        //这一帧过期了,回收所有的BehaviorBuffer
        public readonly Queue<long> timeOutQueue = new();
        public readonly Dictionary<long, BehaviorBuffer> bufferDict = new();

        public long idGenerator;
    }

    //缓冲行为
    public class BehaviorBuffer
    {
        public long Id;
        public string skillTag;
        public long skillOrder;
        public long startFrame;
        public List<string> triggers = new();

        public static BehaviorBuffer Create(long Id, string skillTag, long skillOrder, long startFrame, List<string> _triggers)
        {
            BehaviorBuffer buffer = ObjectPool.Instance.Fetch<BehaviorBuffer>();
            buffer.Id = Id;
            buffer.skillTag = skillTag;
            buffer.skillOrder = skillOrder;
            buffer.startFrame = startFrame;
            _triggers.ForEach(t => { buffer.triggers.Add(t); });
            return buffer;
        }

        public void Recycle()
        {
            this.Id = 0;
            this.skillOrder = 0;
            this.skillTag = "";
            this.startFrame = 0;
            this.triggers.Clear();
        }
    }
}