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

    public class BBAnimViewComponent: MonoBehaviour
    {
        [HideInInspector]
        public long instanceId;

        public BBAnimClip currentClip;

        [Button("测试动画")]
        public void PlayAnim()
        {
            EventSystem.Instance.Invoke(new BBPlayAnim()
            {
                instanceId = this.instanceId, 
                animClip = this.currentClip
            });
        }
    }
}