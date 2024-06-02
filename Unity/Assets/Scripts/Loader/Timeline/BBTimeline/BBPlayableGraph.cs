using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Timeline
{
    [CreateAssetMenu(menuName = "ScriptableObject/BBTimeline/PlayableGraph", fileName = "BBPlayableGraph")]
    public class BBPlayableGraph: SerializedScriptableObject
    {
        [HideReferenceObjectPicker]
        [DictionaryDrawerSettings(KeyLabel = "BehaviorOrder", ValueLabel = "Timeline")]
        [OdinSerialize, NonSerialized]
        public Dictionary<int, BBTimeline> Timelines = new();
    }
}