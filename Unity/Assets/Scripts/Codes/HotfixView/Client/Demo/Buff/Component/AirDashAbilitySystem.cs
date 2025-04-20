using System;

namespace ET.Client
{
    [FriendOf(typeof(AirDashAbility))]
    public static class AirDashAbilitySystem
    {
        public class AirDashAbilityDestroySystem : DestroySystem<AirDashAbility>
        {
            protected override void Destroy(AirDashAbility self)
            {
                self.dashCount = 0;
                self.maxDashCount = 0;
            }
        }
        
        public static int GetDashCount(this AirDashAbility self)
        {
            return self.dashCount;
        }

        public static void SetDashCount(this AirDashAbility self, int value)
        {
            self.dashCount = Math.Clamp(value, 0, self.maxDashCount);
        }

        public static int GetMaxDashCount(this AirDashAbility self)
        {
            return self.maxDashCount;
        }
    }
}