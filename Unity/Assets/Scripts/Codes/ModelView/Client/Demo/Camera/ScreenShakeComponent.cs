using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(VirtualCameraManager))]
    public class ScreenShakeComponent : Entity, IAwake, IDestroy, IFrameLateUpdate
    {
        public float shakeLength_X;
        public float shakeLength_Y;
        public int totalFrame;
        public int curFrame;
        public float frequency;

        public ShakeMode shakeMode;
        public GameObject activeCamera;
    }

    public enum ShakeMode
    {
        Fading,
        Continuous
    }
}