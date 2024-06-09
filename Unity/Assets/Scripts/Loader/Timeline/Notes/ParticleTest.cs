using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Timeline;
using UnityEditor;
using UnityEngine;
using AnimationClip = UnityEngine.AnimationClip;

namespace ET
{
    public class ParticleTest: SerializedMonoBehaviour
    {
        public ParticleSystem particle;

        public AnimationClip clip;

        [Sirenix.OdinInspector.Button("测试")]
        public void TestEdtiorFrame()
        {
            Debug.LogWarning(TimelineUtility.MinEvaluateDeltaTime);
        }

        public void Update()
        {
            Debug.LogWarning(GetComponent<Animator>().deltaPosition);
        }

        [Sirenix.OdinInspector.Button("测试")]
        public void Test()
        {
            AnimationWindow animationWindow = EditorWindow.GetWindow<AnimationWindow>();
            animationWindow.animationClip = clip;
            animationWindow.Show();
        }

        public AnimationClip AnimationClip;

        [OdinSerialize, NonSerialized]
        public Dictionary<string, AnimationCurve> curveDict = new();

        [Sirenix.OdinInspector.Button("测试22")]
        public void Test22()
        {
            curveDict.Clear();
            foreach (var binding in AnimationUtility.GetCurveBindings(AnimationClip))
            {
                string propertyName = binding.propertyName.Replace(".", "_");
                AnimationCurve curve = AnimationUtility.GetEditorCurve(AnimationClip, binding);
                AnimationCurve cloneCurve = MongoHelper.Clone(curve);
                curveDict.Add(propertyName, cloneCurve);
            }
        }

        [Sirenix.OdinInspector.Button("测试33")]
        public void Test33()
        {
            AnimationCurve curve = curveDict["m_LocalPosition_x"];

            float curPos = 0f;
            for (int i = 0; i < 10; i++)
            {
                float x = curve.Evaluate((float)i / TimelineUtility.FrameRate);

                float moveX = x - curPos;
                curPos = x;

                Debug.LogWarning((float)i / TimelineUtility.FrameRate + "  " + moveX);
            }
        }
    }
}