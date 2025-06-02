namespace ET.Client
{
    [FriendOf(typeof(JustEvadeAbility))]
    public static class JustEvadeAbilitySystem
    {
        public class JustEvadeAbilityAwakeSystem : AwakeSystem<JustEvadeAbility, int>
        {
            protected override void Awake(JustEvadeAbility self, int chargeFrame)
            {
                self.chargeFrame = chargeFrame;
                self.cnt = 0;
                self.canJustEvade = true;
            }
        }

        public class JustEvadeAbilityFrameUpdateSystem : FrameUpdateSystem<JustEvadeAbility>
        {
            protected override void FrameUpdate(JustEvadeAbility self)
            {
                if (self.canJustEvade) return;

                if (self.cnt-- <= 0)
                {
                    self.canJustEvade = true;
                }
            }
        }

        public class JustEvadeAbilityDestroySystem : DestroySystem<JustEvadeAbility>
        {
            protected override void Destroy(JustEvadeAbility self)
            {
                self.cnt = 0;
                self.chargeFrame = 0;
                self.canJustEvade = false;
            }
        }

        public static bool CanJustEvade(this JustEvadeAbility self)
        {
            return self.canJustEvade;
        }

        public static void Cost(this JustEvadeAbility self)
        {
            self.canJustEvade = false;
            self.cnt = self.chargeFrame;
        }
    }
}