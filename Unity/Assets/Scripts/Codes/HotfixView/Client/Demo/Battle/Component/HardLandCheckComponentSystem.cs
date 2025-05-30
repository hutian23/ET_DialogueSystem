namespace ET.Client
{
    [FriendOf(typeof(HardLandCheckComponent))]
    public static class HardLandCheckComponentSystem
    {
        public class HardLandCheckComponentDestroySystem : DestroySystem<HardLandCheckComponent>
        {
            protected override void Destroy(HardLandCheckComponent self)
            {
                self.HardLand = false;
                self.cnt = 0;
                self.waitFrame = 0;
                self.airVel = 0f;
            }
        }
        
        public class HardLandCheckComponentAwakeSystem : AwakeSystem<HardLandCheckComponent, int, float>
        {
            protected override void Awake(HardLandCheckComponent self, int waitFrame, float airVel)
            {
                self.waitFrame = waitFrame;
                self.airVel = airVel;
                self.cnt = 0;
                self.HardLand = false;
            }
        }

        public class HardLandCheckComponentPostStepSystem : PostStepSystem<HardLandCheckComponent>
        {
            protected override void PosStepUpdate(HardLandCheckComponent self)
            {
                self.HardLand = false;
                
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);
                if (b2Body.GetVelocity().Y >= self.airVel)
                {
                    self.cnt = 0;
                    return;
                }

                self.HardLand = self.cnt++ >= self.waitFrame;
            }
        }

        public static bool GetHardLand(this HardLandCheckComponent self)
        {
            return self.HardLand;
        }
    }
}