using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 该组件用于管理行为切换
    /// </summary>
    [ComponentOf(typeof(Unit))]
    public class BehaviorMachine: Entity, IAwake, IDestroy
    {
        //当前行为
        public int currentOrder;
        public Dictionary<string, long> behaviorNameMap = new();
        public Dictionary<int, long> behaviorOrderMap = new();
        public List<long> infoList = new();
        public Dictionary<string, long> behaviorFlagDict = new();
    }

    #region 行为机相关事件

    public struct BehaviorReloadCallback
    {
        public long unitId;
        public long infoId; // 传入BehaviorInfo组件的instanceId
    }

    public struct MoveTypeCallback
    {
        public long unitId;
        public long infoId;
    }
    
    public struct LandCallback
    {
        public long instanceId;
    }
    
    #endregion
}