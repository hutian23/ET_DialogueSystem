using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public struct KeyFrameTest
    {
        public BBKeyframe Keyframe;
        public long instanceId;
    }

    public struct BBPlayAnim
    {
        public long instanceId;
        public BBAnimClip animClip;
    }

    public struct BBTimeChanged
    {
        public long instanceId;
        public float timeScale;
    }

    public class BBAnimViewComponent: MonoBehaviour
    {
        [HideInInspector]
        public long instanceId;

        [HideInInspector]
        public long timerComponentInstanceId;

        [LabelText("测试动画")]
        public List<BBAnimClip> currentClip = new();

        [LabelText("运行速度")]
        [Range(0, 1)]
        [OnValueChanged("TimeScaleChanged")]
        public float timeScale = 1;

        [Button("测试动画")]
        public void PlayAnim()
        {
            if (Application.isPlaying)
            {
                // EventSystem.Instance.Invoke(new BBPlayAnim() { instanceId = this.instanceId, animClip = this.currentClip });
            }
        }

        public void TimeScaleChanged()
        {
            if (Application.isPlaying)
            {
                EventSystem.Instance.Invoke(new BBTimeChanged() { instanceId = this.timerComponentInstanceId, timeScale = this.timeScale });
            }
        }
    }
}