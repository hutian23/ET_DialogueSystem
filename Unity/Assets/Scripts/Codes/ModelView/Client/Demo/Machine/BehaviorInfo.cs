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

    public enum MoveType
    {
        None = 0,
        Transition = 1,
        Move = 2,
        Normal = 3,
        Special = 4,
        Super = 5,
        Other = 100, // 非玩家可控制的动作
        HitStun = 101,
        Etc = 102
    }
    
    // 不需要维护MoveType了
    // None = 0, Move = 1, Normal = 3, Special = 5, OverDrive = 6, HitStun = 1000, Etc = 1001;
    // > 1000 为非控制器层
}