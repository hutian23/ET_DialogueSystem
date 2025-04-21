using System;

namespace ET.Client
{
    [FriendOf(typeof(HertzAbility))]
    public static class HertzAbilitySystem
    {
        public class HertzAbilityAwakeSystem : AwakeSystem<HertzAbility>
        {
            protected override void Awake(HertzAbility self)
            {
                self.Hertz = 60;
            }
        }

        public class HertzAbilityDestroySystem : DestroySystem<HertzAbility>
        {
            protected override void Destroy(HertzAbility self)
            {
                self.Hertz = 0;
            }
        }

        //0 - 300
        public static void SetHertz(this HertzAbility self, int hertz)
        {
            int preHertz = self.Hertz;
            self.Hertz = Math.Clamp(hertz, 0, 300);
            
            // Hertz更新，回调事件
            if (preHertz != self.Hertz)
            {
                EventSystem.Instance.Invoke(new HertzChangeCallback(){instanceId = self.InstanceId, hertz = self.Hertz});
            }
        }

        public static int GetHertz(this HertzAbility self)
        {
            return self.Hertz;
        }
    }
}