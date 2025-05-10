using UnityEngine;

namespace ET.Client
{
    public static class GlinShakeComponentSystem
    {
        public class GlinShakeComponentDestroySystem : DestroySystem<GlinShakeComponent>
        {
            protected override void Destroy(GlinShakeComponent self)
            {
                self.shakeLength_X = 0f;
                self.shakeLength_Y = 0f;
                self.cnt = 0;
                
                CinemachineCameraOffset cameraOffset = self.activeCamera.GetComponent<CinemachineCameraOffset>();
                cameraOffset.m_Offset = Vector3.zero;
            }
        }
        
        public class ScreenShakeComponentFrameLateUpdateSystem : FrameLateUpdateSystem<GlinShakeComponent>
        {
            protected override void FrameLateUpdate(GlinShakeComponent self)
            {
                CinemachineCameraOffset cameraOffset = self.activeCamera.GetComponent<CinemachineCameraOffset>();
                cameraOffset.m_Offset = new Vector3(self.shakeLength_X * Mathf.Cos(self.cnt * self.frequency), self.shakeLength_Y * Mathf.Sin(self.cnt * self.frequency), 0);
                self.cnt++;
            }
        }
    }
}