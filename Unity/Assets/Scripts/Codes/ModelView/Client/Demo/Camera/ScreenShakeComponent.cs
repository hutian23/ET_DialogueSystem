using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(VirtualCameraManager))]
    public class ScreenShakeComponent : Entity, IAwake, IDestroy, IPostStep
    {
        public float shakeLength_X;
        public float shakeLength_Y;
        public int totalFrame;
        public int curFrame;
        public float frequency;
        
        public GameObject activeCamera;
    }
}