using System;

namespace ET.Client
{
    public static class GlinFireballAccelSystem
    {
        public class GlinFireballAccelAwakeSystem : AwakeSystem<GlinFireballAccel, float, float>
        {
            protected override void Awake(GlinFireballAccel self, float startY, float accelY)
            {
                self.startY = startY;
                self.accelY = accelY;
            }
        }
        
        public class GlinFireballAccelDestroySystem : DestroySystem<GlinFireballAccel>
        {
            protected override void Destroy(GlinFireballAccel self)
            {
                self.startY = 0;
                self.accelY = 0;
            }
        }
        
        public class GlinFireballAccelFrameUpdateSystem : FrameUpdateSystem<GlinFireballAccel>
        {
            protected override void FrameUpdate(GlinFireballAccel self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);

                float dv = (1 / 60f) * self.accelY;
                float curV = Math.Abs(self.startY) - dv;
                self.startY = Math.Sign(self.startY) * curV;

                b2Body.SetVelocityY(self.startY);
                
                // y轴速度趋于0，销毁组件
                if (curV <= 0.01f)
                {
                    self.Dispose();
                }
            }
        }
    }
}