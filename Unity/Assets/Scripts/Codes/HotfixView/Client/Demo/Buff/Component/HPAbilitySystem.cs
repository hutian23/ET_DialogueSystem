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
                self.MinHP = 0;

                // HP数值更新的默认回调
                Unit unit = self.GetParent<BuffManager>().GetParent<Unit>();
                BBParser parser = unit.GetComponent<BBParser>();
                if (parser.ContainFunction("Root", "HPWatcher"))
                {
                    int functionIndex = parser.GetFunctionPointer("Root", "HPWatcher");
                    self.AddChild<BBAction, long, int>(unit.InstanceId, functionIndex);
                }
            }
        }
        
        public class HPAbilityDestroySystem : DestroySystem<HPAbility>
        {
            protected override void Destroy(HPAbility self)
            {
                self.MaxHP = 0;
                self.CurrentHP = 0;
                self.MinHP = 0;
            }
        }

        public static void SetMinHP(this HPAbility self, int minHP)
        {
            self.MinHP = minHP;
        }

        public static int GetMinHP(this HPAbility self)
        {
            return self.MinHP;
        }
        
        public static void SetHP(this HPAbility self, int HP, bool IsEvent = true)
        {
            int preHP = self.CurrentHP;
            self.CurrentHP = Math.Clamp(HP, self.MinHP, self.MaxHP);

            //回调HP更新事件
            if (self.CurrentHP != preHP && IsEvent)
            {
                EventSystem.Instance.Invoke(new BBActionCallback(){instanceId = self.InstanceId});
            }
        }

        public static int GetHP(this HPAbility self)
        {
            return self.CurrentHP;
        }
    }
}