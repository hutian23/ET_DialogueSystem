namespace ET.Client
{
    public static class WhiffCancelComponentSystem
    {
        [Invoke(EventType.WhiffCancelTimer)]
        [FriendOf(typeof(BehaviorMachine))]
        [FriendOf(typeof(BehaviorInfo))]
        [FriendOf(typeof(WhiffCancelComponent))]
        public class WhiffCancelTimer : BBTimer<Unit>
        {
            protected override void Run(Unit self)
            {
                BehaviorMachine machine = self.GetComponent<BehaviorMachine>();
                WhiffCancelComponent whiff = self.GetComponent<BBParser>().GetComponent<WhiffCancelComponent>();

                int currentOrder = -1;
                foreach (string whiffOption in whiff.Options)
                {
                    BehaviorInfo info = machine.GetInfoByName(whiffOption);
                    //当前挥空取消 不能取消进非控制器层动作
                    if (info.moveType >= MoveType.Other || info.behaviorOrder == 0 || !info.Trigger())
                    {
                        continue;
                    }

                    currentOrder = info.behaviorOrder;
                    break;
                }


                if (currentOrder != -1)
                {
                    machine.Reload(currentOrder);
                }
            }
        }
        
        public class WhiffCancelComponentAwakeSystem : AwakeSystem<WhiffCancelComponent>
        {
            protected override void Awake(WhiffCancelComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
                self.timer = bbTimer.NewFrameTimer(EventType.WhiffCancelTimer, unit);
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