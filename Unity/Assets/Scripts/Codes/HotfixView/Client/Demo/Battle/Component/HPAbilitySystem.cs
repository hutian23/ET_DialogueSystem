using System;

namespace ET.Client
{
    [FriendOf(typeof(HPAbility))]
    public static class HPAbilitySystem
    {
        public class HPAbilityDestroySystem : DestroySystem<HPAbility>
        {
            protected override void Destroy(HPAbility self)
            {
                self.MaxHP = 0;
                self.CurrentHP = 0;
            }
        }

        public static void SetSP(this HPAbility self, int HP)
        {
            int preHP = self.CurrentHP;
            self.CurrentHP = Math.Clamp(HP, 0, self.MaxHP);

            //回调SP更新事件
            if (self.CurrentHP != preHP)
            {
                EventSystem.Instance.Invoke(new SpChangeCallback() { instanceId = self.InstanceId, preSP = preHP, curSP = self.CurrentHP });
            }
        }

        public static int GetSP(this HPAbility self)
        {
            return self.CurrentHP;
        }
    }
}