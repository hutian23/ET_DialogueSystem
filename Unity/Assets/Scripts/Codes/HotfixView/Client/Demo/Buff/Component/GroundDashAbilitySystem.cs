using System;

namespace ET.Client
{
    [FriendOf(typeof(GroundDashAbility))]
    public static class GroundDashAbilitySystem
    {
        public class GroundDashAbilityDestroySystem : DestroySystem<GroundDashAbility>
        {
            protected override void Destroy(GroundDashAbility self)
            {
                self.maxDashCount = 0;
                self.dashCount = 0;
                self.chargeFrame = 0;
            }
        }

        public static void SetDashCount(this GroundDashAbility self, int dashCount)
        {
            int preCount = self.dashCount;
            int curCount = Math.Clamp(dashCount, 0, self.maxDashCount); 
            if (preCount == curCount) return;
            
            //值更新，回调事件
            self.dashCount = curCount;
            EventSystem.Instance.Invoke(new GroundDashChangeCallback(){instanceId = self.InstanceId});
        }

        public static int GetDashCount(this GroundDashAbility self)
        {
            return self.dashCount;
        }

        public static int GetMaxDashCount(this GroundDashAbility self)
        {
            return self.maxDashCount;
        }

        public static int GetChargeFrame(this GroundDashAbility self)
        {
            return self.chargeFrame;
        }
    }
}