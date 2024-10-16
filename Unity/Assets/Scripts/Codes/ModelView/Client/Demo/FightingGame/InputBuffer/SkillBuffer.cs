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
        public Dictionary<int, long> infoDict = new();

        //当前行为
        public int currentOrder;

        public HashSet<string> flags = new();
        public HashSet<string> transitionFlags = new();
        public HashSet<int> GCOptions = new();
    }
}