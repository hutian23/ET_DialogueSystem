using System;

namespace ET.Client
{
    [FriendOf(typeof(HPAbility))]
    public static class HPAbilitySystem
    {
        public class HPAbilityAwakeSystem : AwakeSystem<HPAbility, int>
        {
            protected override void Awake(HPAbility self, int maxHP)
            {
                self.MaxHP = maxHP;
                self.CurrentHP = maxHP;
            }
        }
        
        public class HPAbilityDestroySystem : DestroySystem<HPAbility>
        {
            protected override void Destroy(HPAbility self)
            {
                self.MaxHP = 0;
                self.CurrentHP = 0;
            }
        }

        public static void SetHP(this HPAbility self, int HP, bool IsEvent = true)
        {
            int preHP = self.CurrentHP;
            self.CurrentHP = Math.Clamp(HP, 0, self.MaxHP);

            //回调SP更新事件
            if (self.CurrentHP != preHP && IsEvent)
            {
                EventSystem.Instance.Invoke(new HPChangeCallback() { instanceId = self.InstanceId, preHP = preHP, curHP = self.CurrentHP });
            }
        }

        public static int GetHP(this HPAbility self)
        {
            return self.CurrentHP;
        }
    }
}