using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [Serializable]
    public struct SubBehavior
    {
        public string ClipName;

        [Tooltip("当前行为的动画持续帧数")]
        public long frame;

        [TextArea]
        public string Decsciption;
    }

    [CreateAssetMenu(menuName = "ScriptableObject/BehaviorConfig", fileName = "BehaviorConfig_")]
    public class BehaviorConfig: BaseScriptableObject
    {
        [Space(10)]
        public List<SubBehavior> subBehaviors = new();
    }
}