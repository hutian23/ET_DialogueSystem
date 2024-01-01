using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET.Client
{
    [Serializable]
    public class NodeChecker
    {
        public List<IntParam> intParams = new();
        public List<FloatParam> floatParams = new();
        public List<Vector2Param> vectorParams = new();
        public List<AnimCurParam> animCurParams = new();

        public string CheckerName;

        public int GetInt(string key)
        {
            return this.intParams.FirstOrDefault(i => i.key == key).value;
        }

        public float GetFloat(string key)
        {
            return this.floatParams.FirstOrDefault(i => i.key == key).value;
        }

        public Vector2 GetVector2(string key)
        {
            return this.vectorParams.FirstOrDefault(i => i.key == key).value;
        }

        public AnimationCurve GetAnimationCurve(string key)
        {
            return this.animCurParams.FirstOrDefault(i => i.key == key).value;
        }
    }
}