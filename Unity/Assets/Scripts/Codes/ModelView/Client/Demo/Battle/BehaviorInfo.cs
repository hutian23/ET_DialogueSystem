using System.Collections.Generic;

namespace ET.Client
{
    [ChildOf(typeof (BehaviorMachine))]
    public class BehaviorInfo: Entity, IAwake, IDestroy
    {
        public string behaviorName;
        public int behaviorOrder;
        public MoveType moveType;
        public Dictionary<string, SharedVariable> ParamDict = new(); // 共享变量
    }
    
    // > 100 为非控制器层
    public enum MoveType
    {
        None = 0,
        Transition = 1,
        Move = 2,
        Normal = 3,
        Special = 4,
        Super = 5,
        Other = 100,
        HitStun = 101,
        Death = 102,
        Etc = 103
    }
}