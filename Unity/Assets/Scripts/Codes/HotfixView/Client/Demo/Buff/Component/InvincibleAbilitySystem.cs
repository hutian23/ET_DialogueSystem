namespace ET.Client
{
    [FriendOf(typeof(InvincibleAbility))]
    public static class InvincibleAbilitySystem
    {
        public class InvincibleAbilityAwakeSystem : AwakeSystem<InvincibleAbility, int, long>
        {
            protected override void Awake(InvincibleAbility self, int waitFrame, long unitId)
            {
                self.waitFrame = waitFrame;
                self.cnt = waitFrame;
                self.unitId = unitId;
                b2WorldManager.Instance.GetBody(self.unitId).RegistFilter(FilterType.InvincibleFilter);
            }
        }
        
        public class InvincibleAbilityFrameUpdateSystem : FrameUpdateSystem<InvincibleAbility>
        {
            protected override void FrameUpdate(InvincibleAbility self)
            {
                if (self.cnt-- <= 0) self.Dispose();
            }
        }

        public class InvincibleAbilityDestroySystem : DestroySystem<InvincibleAbility>
        {
            protected override void Destroy(InvincibleAbility self)
            {
                self.waitFrame = 0;
                self.cnt = 0;
                if (b2WorldManager.Instance.ContainBody(self.unitId))
                {
                    b2WorldManager.Instance.GetBody(self.unitId).RemoveFilter(FilterType.InvincibleFilter);
                }
                self.unitId = 0;
            }
        }
    }
}