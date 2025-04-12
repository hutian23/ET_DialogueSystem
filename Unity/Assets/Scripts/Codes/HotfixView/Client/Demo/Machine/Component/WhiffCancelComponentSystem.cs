namespace ET.Client
{
    public static class WhiffCancelComponentSystem
    {
        [Invoke(BBTimerInvokeType.WhiffCancelTimer)]
        [FriendOf(typeof(BehaviorMachine))]
        [FriendOf(typeof(BehaviorInfo))]
        [FriendOf(typeof(WhiffCancelComponent))]
        public class WhiffCancelTimer : BBTimer<Unit>
        {
            protected override void Run(Unit self)
            {
                BehaviorMachine machine = self.GetComponent<BehaviorMachine>();
                WhiffCancelComponent whiff = self.GetComponent<BBParser>().GetComponent<WhiffCancelComponent>();
                BehaviorInfo curInfo = machine.GetInfoByOrder(machine.GetCurrentOrder());

                //1. 找到能够取消的行为
                int currentOrder = -1;
                foreach (string whiffOption in whiff.Options)
                {
                    BehaviorInfo info = machine.GetInfoByName(whiffOption);
                    //当前挥空取消 不能取消进 非控制器层动作 || 待机动作 || 自己
                    if (info == null || info.moveType >= MoveType.Other || info.behaviorOrder == 0 || info.behaviorOrder == curInfo.behaviorOrder)
                    {
                        continue;
                    }

                    if (info.Trigger())
                    {
                        currentOrder = info.behaviorOrder;
                    }
                }
                if (currentOrder == -1)
                {
                    return;
                }

                //2. 进入行为
                machine.Reload(currentOrder);
            }
        }
        
        public class WhiffCancelComponentAwakeSystem : AwakeSystem<WhiffCancelComponent>
        {
            protected override void Awake(WhiffCancelComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
                self.timer = bbTimer.NewFrameTimer(BBTimerInvokeType.WhiffCancelTimer, unit);
            }
        }
        
        public class WhiffCancelComponentDestroySystem : DestroySystem<WhiffCancelComponent>
        {
            protected override void Destroy(WhiffCancelComponent self)
            {
                BBTimerComponent bbTimer = self.GetParent<BBParser>().GetParent<Unit>().GetComponent<BBTimerComponent>();
                bbTimer.Remove(ref self.timer);
                self.Options.Clear();
            }
        }
    }
}