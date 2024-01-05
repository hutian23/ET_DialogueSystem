using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    [Serializable]
    public struct SubBehavior
    {
        public string name;
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
        [InfoBox("1111")]
        public List<SubBehavior> subBehaviors = new();

        public SubBehavior GetSubBehaviorByName(string subBehaviorName)
        {
            return this.subBehaviors.FirstOrDefault(s => s.name == subBehaviorName);
        }
    }
}