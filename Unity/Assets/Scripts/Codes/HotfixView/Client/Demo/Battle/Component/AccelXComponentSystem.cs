namespace ET.Client
{
    public static class AccelXComponentSystem
    {
        public class AccelXComponentAwakeSystem : AwakeSystem<AccelXComponent, float, float, int>
        {
            protected override void Awake(AccelXComponent self, float startX, float accelX, int lastFrame)
            {
                self.startX = startX;
                self.lastFrame = lastFrame;
                self.accelX = accelX;
                self.cnt = 0;
            }
        }
        
        public class AccelXComponentDestroySystem : DestroySystem<AccelXComponent>
        {
            protected override void Destroy(AccelXComponent self)
            {
                self.startX = 0f;
                self.accelX = 0f;
                self.lastFrame = 0;
                self.cnt = 0;
            }
        }
        
        public class AccelXComponentPostStepSystem : PostStepSystem<AccelXComponent>
        {
            protected override void PosStepUpdate(AccelXComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);
                
                float dv = (1 / 60f) * self.accelX;
                float curX = self.startX + dv * self.cnt;
                b2Body.SetVelocityX(curX);
                
                self.cnt++;
                if (self.cnt >= self.lastFrame)
                {
                    self.Dispose();
                }
            }
        }
    }
}