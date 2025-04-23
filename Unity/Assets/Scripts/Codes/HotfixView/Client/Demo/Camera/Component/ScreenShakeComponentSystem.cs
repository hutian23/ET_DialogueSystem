using UnityEngine;

namespace ET.Client
{
    public static class ScreenShakeComponentSystem
    {
        public class ScreenShakeComponentDestroySystem : DestroySystem<ScreenShakeComponent>
        {
            protected override void Destroy(ScreenShakeComponent self)
            {
                self.shakeLength_X = 0f;
                self.shakeLength_Y = 0f;
                self.frequency = 0f;
                self.curFrame = 0;
                self.totalFrame = 0;
                
                CinemachineCameraOffset cameraOffset = self.activeCamera.GetComponent<CinemachineCameraOffset>();
                cameraOffset.m_Offset = Vector3.zero;
            }
        }
        
        public class ScreenShakeComponentPostStepSystem : PostStepSystem<ScreenShakeComponent>
        {
            protected override void PosStepUpdate(ScreenShakeComponent self)
            {
                CinemachineCameraOffset cameraOffset = self.activeCamera.GetComponent<CinemachineCameraOffset>();

                if (self.curFrame-- < 0)
                {
                    self.Dispose();
                    return;
                }
                
                cameraOffset.m_Offset = new Vector3(self.shakeLength_X * Mathf.Cos(self.curFrame * self.frequency) * (self.curFrame / (float)self.totalFrame), self.shakeLength_Y * Mathf.Sin(self.curFrame * self.frequency), 0);
            }
        }
    }
}