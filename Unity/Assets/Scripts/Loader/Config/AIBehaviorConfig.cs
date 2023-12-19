using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [Serializable]
    public class BehaviorConfigInfo
    {
        public string behaviorName;
        public BehaviorConfig config;

        [TextArea]
        public string Desc;
    }
    
    [CreateAssetMenu(menuName = "ScriptableObject/AIConfig", fileName = "AIconfig_")]
    public class AIBehaviorConfig: BaseScriptableObject
    {
        [Space(10)]
        [Header("当前unit的Id，请再UnitConfig.xlsx中查询")]
        public int ConfigId;

        [Header("用于物理检测")]
        public LayerMask CollideMask;

        [Header("每帧执行,处理输入缓冲，检测地面...")]
        public List<CheckerConfig> checkers = new();
        
        [Header("行为机")]
        public List<BehaviorConfigInfo> AIBehaviors = new();
    }
}