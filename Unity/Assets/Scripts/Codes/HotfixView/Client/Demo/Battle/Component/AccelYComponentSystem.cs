namespace ET.Client
{
    public static class AccelYComponentSystem
    {
        public class AccelYComponentAwakeSystem : AwakeSystem<AccelYComponent, float, float, int>
        {
            protected override void Awake(AccelYComponent self, float startY, float accelY, int lastFrame)
            {
                self.startY = startY;
                self.lastFrame = lastFrame;
                self.accelY = accelY;
                self.cnt = 0;
            }
        }
        
        public class AccelYComponentDestroySystem : DestroySystem<AccelYComponent>
        {
            protected override void Destroy(AccelYComponent self)
            {
                self.startY = 0f;
                self.accelY = 0f;
                self.lastFrame = 0;
                self.cnt = 0;
            }
        }
        
        public class AccelYComponentPostStepSystem : PostStepSystem<AccelYComponent>
        {
            protected override void PosStepUpdate(AccelYComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);
                
                float dv = (1 / 60f) * self.accelY;
                float curY = self.startY * dv * self.cnt;
                b2Body.SetVelocityY(curY);
                
                self.cnt++;
                if (self.cnt >= self.lastFrame)
                {
                    self.Dispose();
                }
            }
        }
    }
}