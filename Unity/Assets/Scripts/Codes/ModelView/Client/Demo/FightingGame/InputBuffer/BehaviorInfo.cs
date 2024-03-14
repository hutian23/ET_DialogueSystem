using System.Collections.Generic;

namespace ET.Client
{
    [ChildOf(typeof (BBInputComponent))]
    public class BehaviorInfo: Entity, IAwake, IDestroy
    {
        public uint targetID;
        public int BehaviorType;
        public int order;
        public string tag;
        public string inputChecker;
        public long LastedFrame; // 行为缓冲最大帧数
        public List<string> triggers = new();
    }
}