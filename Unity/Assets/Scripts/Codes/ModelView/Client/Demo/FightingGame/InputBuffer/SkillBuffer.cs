﻿using System.Collections.Generic;

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
        public SortedDictionary<int, long> infoDict = new(Comparer<int>.Create(((x, y) => y.CompareTo(x))));

        //快速通过behaviorName查询到behaviorOrder
        public Dictionary<string, int> behaviorMap = new();
        
        //当前行为
        public int currentOrder;

        //共享变量，当前行为中缓存的一些变量，重载行为时会把缓存的共享变量添加到BBParser组件中
        public Dictionary<string, SharedVariable> paramDict = new();
        public HashSet<int> GCOptions = new();
    }

    public struct ReloadSkillBufferCallback
    {
        public long instanceId;
    }
}