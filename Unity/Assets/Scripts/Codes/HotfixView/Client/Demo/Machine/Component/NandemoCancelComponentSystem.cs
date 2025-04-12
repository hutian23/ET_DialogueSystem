namespace ET.Client
{
    public static class NandemoCancelComponentSystem
    {
        // 对应IASA，一般用于取消一些过渡动画，当前动作可被所有动作取消
        [Invoke(BBTimerInvokeType.NandemoCancelTimer)]
        [FriendOf(typeof(BehaviorInfo))]
        [FriendOf(typeof(BehaviorMachine))]
        public class NandemoCancelTimer : BBTimer<Unit>
        {
            protected override void Run(Unit self)
            {
                BehaviorMachine machine = self.GetComponent<BehaviorMachine>();
            
                //1. 
                int currentOrder = machine.GetCurrentOrder();
                for(int i = machine.infoList.Count - 1; i > 0; i--)
                {
                    BehaviorInfo info = machine.GetChild<BehaviorInfo>(machine.infoList[i]);
                    //非控制器层的动作
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
                self.timer = bbTimerComponent.NewFrameTimer(BBTimerInvokeType.NandemoCancelTimer,unit);
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