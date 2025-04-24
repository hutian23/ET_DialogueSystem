namespace ET.Client
{
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(BehaviorMachine))]
    [FriendOf(typeof(BBTimerComponent))]
    public static class BehaviorMachineSystem
    {
        public class BehaviorBufferAwakeSystem : AwakeSystem<BehaviorMachine>
        {
            protected override void Awake(BehaviorMachine self)
            {
                self.Init();
            }
        }
        
        public class BehaviorBufferDestroySystem : DestroySystem<BehaviorMachine>
        {
            protected override void Destroy(BehaviorMachine self)
            {
                self.Cancel();
            }
        }

        private static void Cancel(this BehaviorMachine self)
        {
            self.Token.Cancel();
            self.currentOrder = -1;
            self.behaviorOrderMap.Clear();
            self.behaviorNameMap.Clear();
            self.infoList.Clear();
            self.behaviorFlagDict.Clear();
        }

        private static void Init(this BehaviorMachine self)
        {
            self.Cancel();
            self.Token = new();
        }

        public static void SetCurrentOrder(this BehaviorMachine self, int order)
        {
            self.currentOrder = order;
        }

        public static int GetCurrentOrder(this BehaviorMachine self)
        {
            return self.currentOrder;
        }

        public static BehaviorInfo GetInfoByOrder(this BehaviorMachine self, int behaviorOrder)
        {
            if (!self.behaviorOrderMap.TryGetValue(behaviorOrder, out long infoId))
            {
                Log.Error($"does not exist behavior, Order: {behaviorOrder}");
                return null;
            }
            return self.GetChild<BehaviorInfo>(infoId);
        }

        public static BehaviorInfo GetInfoByName(this BehaviorMachine self, string behaviorName)
        {
            if (!self.behaviorNameMap.TryGetValue(behaviorName, out long infoId))
            {
                Log.Error($"does not exist behavior, Name: {behaviorName}");
                return null;
            }
            return self.GetChild<BehaviorInfo>(infoId);
        }

        public static BehaviorInfo GetInfoByFlag(this BehaviorMachine self, string behaviorFlag)
        {
            if (!self.behaviorFlagDict.TryGetValue(behaviorFlag, out long infoId))
            {
                Log.Error($"does not exist behavior, flag: {behaviorFlag}");
                return null;
            }

            return self.GetChild<BehaviorInfo>(infoId);
        }

        public static void Reload(this BehaviorMachine self, string behaviorName)
        {
            BehaviorInfo info = self.GetInfoByName(behaviorName);
            EventSystem.Instance.Invoke(new BehaviorReloadCallback(){ unitId = self.GetParent<Unit>().InstanceId, infoId = info.InstanceId });
        }

        public static void Reload(this BehaviorMachine self, int behaviorOrder)
        {
            BehaviorInfo info = self.GetInfoByOrder(behaviorOrder);
            EventSystem.Instance.Invoke(new BehaviorReloadCallback(){ unitId = self.GetParent<Unit>().InstanceId, infoId = info.InstanceId });
        }
    }
}