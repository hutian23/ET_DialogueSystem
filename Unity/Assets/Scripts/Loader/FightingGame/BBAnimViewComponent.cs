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

        public BBAnimClip currentClip;

        [LabelText("运行速度")]
        [Range(0, 1)]
        [OnValueChanged("TimeScaleChanged")]
        public float timeScale;

        [Button("测试动画")]
        public void PlayAnim()
        {
            EventSystem.Instance.Invoke(new BBPlayAnim() { instanceId = this.instanceId, animClip = this.currentClip });
        }

        public void TimeScaleChanged()
        {
            EventSystem.Instance.Invoke(new BBTimeChanged() { instanceId = this.timerComponentInstanceId, timeScale = this.timeScale });
        }
    }
}