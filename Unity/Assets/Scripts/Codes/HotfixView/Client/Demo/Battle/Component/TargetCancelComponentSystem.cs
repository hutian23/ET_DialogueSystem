namespace ET.Client
{
    [FriendOf(typeof(TargetCancelComponent))]
    public static class TargetCancelComponentSystem
    {
        [Invoke(EventType.TargetCancelTimer)]
        [FriendOf(typeof(TargetCancelComponent))]
        [FriendOf(typeof(BehaviorInfo))]
        public class TargetCancelTimer : BBTimer<Unit>
        {
            protected override void Run(Unit self)
            {
                BehaviorMachine machine = self.GetComponent<BehaviorMachine>();
                TargetCancelComponent tc = self.GetComponent<BBParser>().GetComponent<TargetCancelComponent>();
                
                //1.  
                int currentOrder = -1;
                foreach (string option in tc.Options)
                {
                    BehaviorInfo info = machine.GetInfoByName(option);
                    if (info == null || info.moveType >= MoveType.Other || info.behaviorOrder == 0)
                    {
                        continue;
                    }

                    if (info.TC_Trigger())
                    {
                        currentOrder = info.behaviorOrder;
                        break;
                    }
                }
                if (currentOrder == -1)
                {
                    return;
                }

                //2. 进入动作
                machine.Reload(currentOrder);
            }
        }

        public class TargetCancelAwakeSystem : AwakeSystem<TargetCancelComponent>
        {
            protected override void Awake(TargetCancelComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
                self.timer = bbTimer.NewFrameTimer(EventType.TargetCancelTimer, unit);
            }
        }

        public class TargetCancelDestroySystem : DestroySystem<TargetCancelComponent>
        {
            protected override void Destroy(TargetCancelComponent self)
            {
                BBTimerComponent bbTimer = self.GetParent<BBParser>().GetParent<Unit>().GetComponent<BBTimerComponent>();
                bbTimer.Remove(ref self.timer);
                self.Options.Clear();
            }
        }

        public static void Add(this TargetCancelComponent self, string option)
        {
            self.Options.Add(option);
        }

        public static bool Contain(this TargetCancelComponent self, string option)
        {
            return self.Options.Contains(option);
        }
    }
}