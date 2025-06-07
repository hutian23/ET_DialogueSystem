using Box2DSharp.Dynamics;

namespace ET.Client
{
    public static class B2UnitSystem
    {
        public class B2UnitAwakeSystem : AwakeSystem<B2Unit>
        {
            protected override void Awake(B2Unit self)
            {
                self.unitId = self.GetParent<Unit>().InstanceId;
                BodyDef def = new()
                {
                    BodyType = BodyType.DynamicBody,
                    GravityScale = 0f,
                    LinearDamping = 0f,
                    AngularDamping = 0f,
                    AllowSleep = true,
                    FixedRotation = true,
                };
                b2Body b2Body = b2WorldManager.Instance.CreateBody(self.unitId, def);
                b2Body.RegistFilter(FilterType.PushBoxFilter);
            }
        }
        
        public class B2UnitDestroySystem : DestroySystem<B2Unit>
        {
            protected override void Destroy(B2Unit self)
            {
                b2WorldManager.Instance.DestroyBody(self.unitId);   
            }
        }
    }
}