using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 该组件用于管理行为切换
    /// </summary>
    [ComponentOf(typeof (TimelineComponent))]
    public class SkillBuffer: Entity, IAwake, IDestroy
    {
        public long CheckTimer;

        //BehaviorOrder ---> EntityId
        public Dictionary<int, long> infoDict = new();

        //当前行为
        public int currentOrder;

        //共享变量，当前行为中缓存的一些变量，重载行为时会把缓存的共享变量添加到BBParser组件中
        public Dictionary<string, SharedVariable> paramDict = new();
        public HashSet<int> GCOptions = new();
    }
}