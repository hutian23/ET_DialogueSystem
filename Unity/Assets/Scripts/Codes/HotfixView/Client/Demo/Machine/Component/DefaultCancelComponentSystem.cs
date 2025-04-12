namespace ET.Client
{
    public static class DefaultCancelComponentSystem
    {
        [Invoke(BBTimerInvokeType.DefaultCancelTimer)]
        [FriendOf(typeof(BehaviorMachine))]
        [FriendOf(typeof(BehaviorInfo))]
        public class DefaultCancelTimer : BBTimer<Unit>
        {
            protected override void Run(Unit self)
            {
                BehaviorMachine machine = self.GetComponent<BehaviorMachine>();
                
                //1. 
                int currentOrder = machine.GetCurrentOrder();
                for (int i = machine.infoList.Count - 1; i > machine.GetCurrentOrder(); i--)
                {
                    BehaviorInfo info = machine.GetChild<BehaviorInfo>(machine.infoList[i]);
                    if (info.moveType >= MoveType.Other)
                    {
                        continue;
                    }
                    if (info.Trigger())
                    {
                        currentOrder = info.behaviorOrder;
                        break;
                    }
                }
                if (currentOrder == machine.GetCurrentOrder())
                {
                    return;
                }
                
                //2. 进入行为
                machine.Reload(currentOrder);
            }
        }
        
        public class DefaultCancelAwakeSystem : AwakeSystem<DefaultCancelComponent>
        {
            protected override void Awake(DefaultCancelComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                BBTimerComponent bbTimer = self.GetComponent<BBTimerComponent>();
                self.timer = bbTimer.NewFrameTimer(BBTimerInvokeType.DefaultCancelTimer, unit);
            }
        }
        
        public class DefaultCancelDestroySystem : DestroySystem<DefaultCancelComponent>
        {
            protected override void Destroy(DefaultCancelComponent self)
            {
                BBTimerComponent bbTimer = self.GetParent<BBParser>().GetParent<Unit>().GetComponent<BBTimerComponent>();
                bbTimer.Remove(ref self.timer);
            }
        }
    }
}