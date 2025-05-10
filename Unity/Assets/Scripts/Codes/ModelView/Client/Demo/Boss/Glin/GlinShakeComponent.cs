using UnityEngine;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class GlinShakeComponent : Entity, IAwake, IDestroy, IFrameLateUpdate
    {
        public float shakeLength_X;
        public float shakeLength_Y;
        public float frequency;
        public int cnt;

        public GameObject activeCamera;
    }
}