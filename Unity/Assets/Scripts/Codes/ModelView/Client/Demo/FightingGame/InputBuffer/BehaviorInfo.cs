using System.Collections.Generic;

namespace ET.Client
{
    // [ComponentOf(typeof())]
    public class BehaviorInfo: Entity, IAwake, IDestroy
    {
        public long skillOrder;

        //每帧检测计时器
        public long checkTimer;
        public int checkType;

        public List<string> triggers = new();
    }
}