using System;

namespace ET.Client
{
    [FriendOf(typeof(SPAbility))]
    public static class SPAbilitySystem
    {
        public class SPAbilityDestroySystem : DestroySystem<SPAbility>
        {
            protected override void Destroy(SPAbility self)
            {
                self.MaxSP = 0;
                self.CurrentSP = 0;
            }
        }

        public static void SetSP(this SPAbility self, int SP)
        {
            int preSP = self.CurrentSP;
            self.CurrentSP = Math.Clamp(SP, 0, self.MaxSP);
            
            //回调SP更新事件
            if (self.CurrentSP != preSP)
            {
                EventSystem.Instance.Invoke(new SpChangeCallback() { instanceId = self.InstanceId, preSP = preSP, curSP = self.CurrentSP });
            }
        }

        public static int GetSP(this SPAbility self)
        {
            return self.CurrentSP;
        }
    }
}