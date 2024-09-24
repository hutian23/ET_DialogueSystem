using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    [CreateAssetMenu(menuName = "ScriptableObject/BBAnim", fileName = "BBAnimClip_")]
    public class BBAnimClip: SerializedScriptableObject
    {
        public bool IsLoop;

        [DictionaryDrawerSettings(KeyLabel = "FrameName: "), ShowInInspector]
        public List<BBKeyframe> Keyframes = new();

        [Button("动画编辑器")]
        public void EnterAnimEditor()
        {
        }
    }
}