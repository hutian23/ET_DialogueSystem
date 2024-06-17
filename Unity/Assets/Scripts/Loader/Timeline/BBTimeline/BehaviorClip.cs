using System;
using UnityEngine;

namespace Timeline
{
    [Serializable]
    public class BehaviorClip
    {
        [TextArea(10, 30)]
        public string Script;

#if UNITY_EDITOR
        public Vector3 ClipPos;
#endif
    }
}