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
        [OdinSerialize, NonSerialized]
        public List<BBTimeline> Timelines = new();
    }
}