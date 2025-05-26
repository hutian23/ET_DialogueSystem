namespace ET.Client
{
    public static class NandemoCancelComponentSystem
    {
        // 对应IASA，一般用于取消一些过渡动画，当前动作可被所有动作取消
        [Invoke(EventType.NandemoCancelTimer)]
        [FriendOf(typeof(BehaviorInfo))]
        [FriendOf(typeof(BehaviorMachine))]
        public class NandemoCancelTimer : BBTimer<Unit>
        {
            protected override void Run(Unit self)
            {
                BehaviorMachine machine = self.GetComponent<BehaviorMachine>();
            
                //1. 可以被除了Idle以外的所有动作取消
                int currentOrder = machine.GetCurrentOrder();
                for(int i = machine.infoList.Count - 1; i > 0; i--)
                {
                    BehaviorInfo info = machine.GetChild<BehaviorInfo>(machine.infoList[i]);
                    if (info.moveType >= MoveType.Other)
                    {
                        continue;
                    }
                
                    //符合前置条件
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
                
           
                //2. 重载行为
                machine.Reload(currentOrder);
            }
        }
        
        public class NandemoCancelAwakeSystem : AwakeSystem<NandemoCancelComponent>
        {
            protected override void Awake(NandemoCancelComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                BBTimerComponent bbTimerComponent = unit.GetComponent<BBTimerComponent>();
                self.timer = bbTimerComponent.NewFrameTimer(EventType.NandemoCancelTimer,unit);
            }
        }
        
        public class NandemoCancelDestroySystem : DestroySystem<NandemoCancelComponent>
        {
            protected override void Destroy(NandemoCancelComponent self)
            {
                BBTimerComponent bbTimer = self.GetParent<BBParser>().GetParent<Unit>().GetComponent<BBTimerComponent>();
                bbTimer.Remove(ref self.timer);
            }
        }
    }
}