using System;
using System.Collections.Generic;
using ET.Client;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace Timeline
{
    [CreateAssetMenu(menuName = "ScriptableObject/BBTimeline/PlayableGraph", fileName = "BBPlayableGraph")]
    public class BBPlayableGraph: SerializedScriptableObject
    {
#if UNITY_EDITOR
        private SerializedObject SerializedController;
        public void SerializedUpdate()
        {
            SerializedController = new SerializedObject(this);
            SerializedController.Update();
        }
#endif

        public RootClip root;

        [HideReferenceObjectPicker]
        [OdinSerialize, NonSerialized]
        public List<BehaviorLayer> Layers = new();

        [HideReferenceObjectPicker]
        [OdinSerialize, NonSerialized]
        public List<SharedVariable> Parameters = new();
    }

    [Serializable]
    public class RootClip
    {
        [TextArea(10, 30)]
        public string MainScript;
    }
}