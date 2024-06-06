using System;
using System.Collections.Generic;
using UnityEngine;

namespace Timeline.Editor
{
#if UNITY_EDITOR
    public class BasicExpressionInfo
    {
        public bool m_foldoutAnimation;
        public bool m_foldoutPredefine;
        public List<FloatAttr> m_baseExpressionFloat = new();
        public List<StringArr> m_baseExpressionString = new();
        public List<int> m_baseExpressionIndex = new();
    }

    [Serializable]
    public class FloatAttr
    {
        public float[] m_value;
    }

    [Serializable]
    public class StringArr
    {
        public string[] m_value;
    }
    
#endif

    public class Basic: MonoBehaviour
    {
#if UNITY_EDITOR
        public BasicExpressionInfo m_info = new();
#endif
        public UnityEngine.AnimationClip m_clip;
        public List<string> m_baseExpressionName = new();
        public List<float> m_baseExpressionValue = new();
    }
}