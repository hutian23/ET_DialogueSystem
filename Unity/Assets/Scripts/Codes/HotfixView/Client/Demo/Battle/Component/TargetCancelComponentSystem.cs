namespace ET.Client
{
    public static class TargetCancelComponentSystem
    {
        [Invoke(EventType.TargetCancelTimer)]
        [FriendOf(typeof(TargetComboCancelComponent))]
        [FriendOf(typeof(BehaviorInfo))]
        [FriendOf(typeof(ComboOffsetAbility))]
        public class TargetCancelTimer : BBTimer<Unit>
        {
            protected override void Run(Unit self)
            {
                BehaviorMachine machine = self.GetComponent<BehaviorMachine>();
                ComboOffsetAbility ability = self.GetComponent<BuffManager>().GetComponent<ComboOffsetAbility>();
        
                int count = ability.bufferQueue.Count;
                while (count-- > 0)
                {
                    ComboOffsetBuffer buffer = ability.bufferQueue.Dequeue();
        
                    // 查询behaviorInfo
                    BehaviorInfo info = machine.GetInfoByName(buffer.behaviorName);
                    if (info.moveType >= MoveType.Other || info.behaviorOrder == 0)
                    {
                        ability.bufferQueue.Enqueue(buffer);
                        continue;
                    }
        
                    // 技能的代码块中需要声明 TargetComboTrigger()
                    if (info.TargetComboTrigger())
                    {
                        machine.Reload(buffer.behaviorName);
                        return;
                    }
        
                    // 如果TargetCancel中使用了这个offsetBuffer，从队列中移除
                    ability.bufferQueue.Enqueue(buffer);
                }
            }
        }

        public class TargetCancelAwakeSystem : AwakeSystem<TargetComboCancelComponent>
        {
            protected override void Awake(TargetComboCancelComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
                self.timer = bbTimer.NewFrameTimer(EventType.TargetCancelTimer, unit);
            }
        }

        public class TargetCancelDestroySystem : DestroySystem<TargetComboCancelComponent>
        {
            protected override void Destroy(TargetComboCancelComponent self)
            {
                BBTimerComponent bbTimer = self.GetParent<BBParser>().GetParent<Unit>().GetComponent<BBTimerComponent>();
                bbTimer.Remove(ref self.timer);
            }
        }
    }
}