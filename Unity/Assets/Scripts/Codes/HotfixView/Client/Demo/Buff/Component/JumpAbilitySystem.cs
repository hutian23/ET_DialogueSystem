using System;

namespace ET.Client
{
    [FriendOf(typeof(JumpAbility))]
    public static class JumpAbilitySystem
    {
        public class JumpAbilityDestroySystem : DestroySystem<JumpAbility>
        {
            protected override void Destroy(JumpAbility self)
            {
                self.JumpCount = 0;
                self.JumpMaxCount = 0;
            }
        }

        public static int GetJumpCount(this JumpAbility self)
        {
            return self.JumpCount;
        }

        public static int GetMaxJumpCount(this JumpAbility self)
        {
            return self.JumpMaxCount;
        }

        public static void SetJumpCount(this JumpAbility self, int jumpCount)
        {
            self.JumpCount = Math.Clamp(jumpCount, 0, self.JumpMaxCount);
        }
    }
}