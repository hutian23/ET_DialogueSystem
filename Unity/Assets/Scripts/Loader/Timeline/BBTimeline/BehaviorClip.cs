using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Timeline
{
    [Serializable]
    public class BehaviorClip
    {
        public BBTimeline Timeline;
        public string Title;

#if UNITY_EDITOR
        [HideInInspector]
        public string viewDataKey;

        [HideInInspector]
        public Vector3 ClipPos;
#endif
    }

    [Serializable]
    public class BehaviorLayer
    {
        public string layerName;

        [HideReferenceObjectPicker]
        public List<BehaviorClip> BehaviorClips = new();

#if UNITY_EDITOR
        [FormerlySerializedAs("linkData")]
        [HideReferenceObjectPicker]
        public List<BehaviorLinkData> linkDatas = new();
#endif
    }

#if UNITY_EDITOR
    [Serializable]
    public class BehaviorLinkData
    {
        public string viewDataKey;
        public string inputGuid;
        public string outputGuid;
    }
#endif
}