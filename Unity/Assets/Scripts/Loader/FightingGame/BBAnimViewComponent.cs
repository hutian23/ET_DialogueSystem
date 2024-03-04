using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    public struct KeyFrameTest
    {
        public BBKeyframe Keyframe;
        public long instanceId;
    }

    public class BBAnimViewComponent: MonoBehaviour
    {
        [HideInInspector]
        public long instanceId;

        public BBAnimClip Clip;

        public BBKeyframe keyFrame;

        [Button("测试KeyFrame")]
        public void Test()
        {
            EventSystem.Instance.Invoke(new KeyFrameTest() { instanceId = this.instanceId, Keyframe = this.keyFrame });
        }
    }
}