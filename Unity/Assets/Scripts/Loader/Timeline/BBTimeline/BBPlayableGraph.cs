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
        [HideReferenceObjectPicker]
        [OdinSerialize, NonSerialized]
        public List<BehaviorClip> BehaviorClips = new();

#if UNITY_EDITOR
        public List<BehaviorLinkData> linkDatas = new();

        private SerializedObject SerializedController;
        public void SerializedUpdate()
        {
            SerializedController = new SerializedObject(this);
            SerializedController.Update();
        }
#endif

        public RootClip root;

        public List<String> Layers = new();
    }

    [Serializable]
    public class RootClip
    {
        [TextArea(10, 30)]
        public string MainScript;
    }
}