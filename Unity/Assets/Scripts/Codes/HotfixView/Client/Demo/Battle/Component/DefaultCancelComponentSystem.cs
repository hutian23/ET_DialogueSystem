namespace ET.Client
{
    public static class DefaultCancelComponentSystem
    {
        [Invoke(EventType.DefaultCancelTimer)]
        [FriendOf(typeof(BehaviorMachine))]
        [FriendOf(typeof(BehaviorInfo))]
        [FriendOf(typeof(ComboOffsetAbility))]
        public class DefaultCancelTimer : BBTimer<Unit>
        {
            protected override void Run(Unit self)
            {
                BehaviorMachine machine = self.GetComponent<BehaviorMachine>();
                ComboOffsetAbility ability = self.GetComponent<BuffManager>().GetComponent<ComboOffsetAbility>();

                //1. ComboOffset中缓存的动作优先级最高
                int count = ability.bufferQueue.Count;
                while (count -- > 0)
                {
                    ComboOffsetBuffer buffer = ability.bufferQueue.Dequeue();
                    ability.bufferQueue.Enqueue(buffer);
                 
                    // 查询behaviorInfo
                    BehaviorInfo info = machine.GetInfoByName(buffer.behaviorName);
                    if (info.moveType >= MoveType.Other || info.behaviorOrder == 0)
                    {
                        continue;
                    }

                    // 技能的代码块中需要声明 TargetComboTrigger()
                    if (info.TargetComboTrigger())
                    {
                        machine.Reload(buffer.behaviorName);
                        return;
                    }
                }
                
                //2. 当前动作可以被所有优先级更高的动作取消
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
                BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
                self.timer = bbTimer.NewFrameTimer(EventType.DefaultCancelTimer, unit);
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