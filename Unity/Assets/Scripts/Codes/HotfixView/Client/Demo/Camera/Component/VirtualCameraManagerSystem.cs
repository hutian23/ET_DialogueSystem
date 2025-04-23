using Cinemachine;
using UnityEngine;

namespace ET.Client
{
    public static class VirtualCameraManagerSystem
    {
        public class VirtualCameraAwakeSystem : AwakeSystem<VirtualCameraManager>
        {
            protected override void Awake(VirtualCameraManager self)
            {
                VirtualCameraManager.Instance = self;
            }
        }
        
        public class VirtualCameraDestroySystem : DestroySystem<VirtualCameraManager>
        {
            protected override void Destroy(VirtualCameraManager self)
            {
                VirtualCameraManager.Instance = null;
            }
        }
        
        public class VirtualCameraLateUpdateSystem : FrameLateUpdateSystem<VirtualCameraManager>
        {
            protected override void FrameLateUpdate(VirtualCameraManager self)
            {
                CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
                brain.ManualUpdate();
            }
        }
    }
}