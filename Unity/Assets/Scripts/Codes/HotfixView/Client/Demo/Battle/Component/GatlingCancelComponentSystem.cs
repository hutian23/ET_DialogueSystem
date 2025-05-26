namespace ET.Client
{
    [FriendOf(typeof(GatlingCancelComponent))]
    public static class GatlingCancelComponentSystem
    {
        [Invoke(EventType.GatlingCancelTimer)]
        [FriendOf(typeof(BehaviorMachine))]
        [FriendOf(typeof(BehaviorInfo))]
        [FriendOf(typeof(GatlingCancelComponent))]
        public class GatlingCancelTimer : BBTimer<Unit>
        {
            protected override void Run(Unit self)
            {
                BehaviorMachine machine = self.GetComponent<BehaviorMachine>();
                GatlingCancelComponent gc = self.GetComponent<BBParser>().GetComponent<GatlingCancelComponent>();
                BehaviorInfo curInfo = machine.GetInfoByOrder(machine.GetCurrentOrder());

                //1. 
                int currentOrder = -1;
                for (int i = machine.infoList.Count - 1; i >= 0; i--)
                {
                    BehaviorInfo info = machine.GetChild<BehaviorInfo>(machine.infoList[i]);
                    if (info.moveType >= MoveType.Other)
                    {
                        continue;
                    }
                    //只能被层级高于当前动作 or 添加了特殊取消标签的动作取消
                    if ((info.moveType > curInfo.moveType || gc.Options.Contains(info.behaviorName)) && info.Trigger())
                    {
                        currentOrder = info.behaviorOrder;
                        break;
                    }
                }
                if (currentOrder == -1)
                {
                    return;
                }

                //2. 动作切换
                machine.Reload(currentOrder);
            }
        }

        public class GatlingCancelAwakeSystem : AwakeSystem<GatlingCancelComponent>
        {
            protected override void Awake(GatlingCancelComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
                self.timer = bbTimer.NewFrameTimer(EventType.GatlingCancelTimer, unit);
            }
        }

        public class GatlingCancelDestroySystem : DestroySystem<GatlingCancelComponent>
        {
            protected override void Destroy(GatlingCancelComponent self)
            {
                BBTimerComponent bbTimer = self.GetParent<BBParser>().GetParent<Unit>().GetComponent<BBTimerComponent>();
                bbTimer.Remove(ref self.timer);
                self.Options.Clear();
            }
        }
    }
}