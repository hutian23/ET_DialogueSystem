using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET
{
    [Serializable]
    public struct intParam
    {
        public string key;
        public int value;

        [TextArea]
        public string desc;
    }

    [Serializable]
    public struct Vector2Param
    {
        public string key;
        public Vector2 value;

        [TextArea]
        public string desc;
    }

    [Serializable]
    public struct RectParam
    {
        public string key;
        public Rect value;

        [TextArea]
        public string desc;
    }

    [Serializable]
    public struct FloatParam
    {
        public string key;
        public float value;

        [TextArea]
        public string desc;
    }

    [Serializable]
    public struct LayerParam
    {
        public string key;
        public LayerMask value;

        [TextArea]
        public string desc;
    }

    public class BaseScriptableObject: ScriptableObject
    {
        public List<intParam> data_int = new();

        public List<FloatParam> data_float = new();

        public List<RectParam> data_rect = new();

        public List<Vector2Param> data_vector = new();

        public List<LayerParam> data_Layer = new();

        public int GetInt(string key)
        {
            intParam param = this.data_int.FirstOrDefault(i => i.key == key);
            if (string.IsNullOrEmpty(param.key))
            {
                Debug.LogWarning($"不存在int形参数:{key}");
                return 0;
            }

            return param.value;
        }

        public float GetFloat(string key)
        {
            FloatParam param = this.data_float.FirstOrDefault(i => i.key == key);
            if (string.IsNullOrEmpty(param.key))
            {
                Debug.LogWarning($"不存在float型参数 {key}");
                return 0f;
            }

            return param.value;
        }

        public Vector2 GetVector2(string key)
        {
            Vector2Param param = this.data_vector.FirstOrDefault(i => i.key == key);
            if (string.IsNullOrEmpty(param.key))
            {
                Debug.LogWarning($"不存在Vector2型参数 {key}");
            }

            return param.value;
        }

        public Rect GetRect(string key)
        {
            RectParam param = this.data_rect.FirstOrDefault(i => i.key == key);
            if (string.IsNullOrEmpty(param.key))
            {
                Debug.LogWarning($"不存在rect型参数 {key}");
                return Rect.zero;
            }

            return param.value;
        }

        public LayerMask GetLayer(string key)
        {
            LayerParam param = this.data_Layer.FirstOrDefault(i => i.key == key);
            if (string.IsNullOrEmpty(param.key))
            {
                Debug.LogWarning($"不存在Layer参数: {key}");
                return default;
            }

            return param.value;
        }
    }
}