using System.Numerics;

namespace ET.Client
{
    [FriendOf(typeof(B2Unit))]
    public static class B2UnitSystem
    {
        public class B2UnitAwakeSystem : AwakeSystem<B2Unit,long>
        {
            protected override void Awake(B2Unit self,long unitId)
            {
                self.unitId = unitId;
                EventSystem.Instance.Invoke(new CreateB2bodyCallback(){instanceId = self.unitId});
            }
        }

        public class B2UnitPreStepSystem : PreStepSystem<B2Unit>
        {
            protected override void PreStepUpdate(B2Unit self)
            {
                b2Body body = b2WorldManager.Instance.GetBody(self.unitId);
                body.SetVelocity(self.Velocity * new Vector2(- body.GetFlip(), 1) * (self.Hertz / 60f));
            }
        }
        
        public class B2UnitPostStepSystem : PostStepSystem<B2Unit>
        {
            protected override void PosStepUpdate(B2Unit self)
            {
                //清空碰撞缓冲区
                self.TriggerBuffer.Clear();
                self.CollisionBuffer.Clear();
            }
        }
        
        public static Vector2 GetVelocity(this B2Unit self)
        {
            return self.Velocity;
        }

        public static void SetVelocity(this B2Unit self, Vector2 velocity)
        {
            self.Velocity = velocity;
        }
        
        public static void SetVelocityY(this B2Unit self, float velocityY)
        {
            self.SetVelocity(new Vector2(self.Velocity.X, velocityY));
        }

        public static void SetVelocityX(this B2Unit self, float velocityX)
        {
            self.SetVelocity(new Vector2(velocityX, self.Velocity.Y));
        }
        
        public static int GetHertz(this B2Unit self)
        {
            return self.Hertz;
        }

        public static void SetHertz(this B2Unit self, int hertz)
        {
            self.Hertz = hertz;
        }
    }
}