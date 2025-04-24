using Cinemachine;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(VirtualCameraManager))]
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
                self.cameraList.Clear();
                
                //销毁Go
                UnityEngine.Object.Destroy(self.Global);
                UnityEngine.Object.Destroy(self.Target);
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

        public static void AddCamera(this VirtualCameraManager self, string cameraName, VirtualCamera camera)
        {
            if (!self.cameraList.TryAdd(cameraName, camera.Id))
            {
                Log.Error($"already exist camera : {cameraName}");
            }
        }
        
        public static VirtualCamera GetCamera(this VirtualCameraManager self, string cameraName)
        {
            if (!self.cameraList.TryGetValue(cameraName, out long Id))
            {
                Log.Error($"cannot found virtualCamera: {cameraName}");
                return null;
            }
            
            return self.GetChild<VirtualCamera>(Id);
        }
    }
}