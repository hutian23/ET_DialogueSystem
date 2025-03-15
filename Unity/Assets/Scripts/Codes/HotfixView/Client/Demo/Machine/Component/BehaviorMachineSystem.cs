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
            foreach (var kv in self.paramDict)
            {
                self.paramDict[kv.Key].Recycle();
            }
            self.paramDict.Clear();
            foreach (var kv in self.tmpParamDict)
            {
                self.tmpParamDict[kv.Key].Recycle();
            }
            self.tmpParamDict.Clear();
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
        
        #region Param

        public static T RegistParam<T>(this BehaviorMachine self, string paramName, T value)
        {
            if (self.paramDict.ContainsKey(paramName))
            {
                Log.Error($"already contain params:{paramName}");
                return default;
            }

            SharedVariable variable = SharedVariable.Create(paramName, value);
            self.paramDict.Add(paramName, variable);
            return value;
        }

        public static T RegistTmpParam<T>(this BehaviorMachine self, string paramName, T value)
        {
            if (self.tmpParamDict.ContainsKey(paramName))
            {
                Log.Error($"already contain params:{paramName}");
                return default;
            }

            SharedVariable variable = SharedVariable.Create(paramName, value);
            self.tmpParamDict.Add(paramName, variable);
            return value;
        }
        
        public static T GetParam<T>(this BehaviorMachine self, string paramName)
        {
            if (!self.paramDict.TryGetValue(paramName, out SharedVariable variable))
            {
                Log.Error($"does not exist param:{paramName}!");
                return default;
            }

            if (variable.value is not T value)
            {
                Log.Error($"cannot format {variable.name} to {typeof(T)}");
                return default;
            }
            return value;
        }

        public static bool ContainParam(this BehaviorMachine self, string paramName)
        {
            return self.paramDict.ContainsKey(paramName);
        }

        public static bool ContainTmpParam(this BehaviorMachine self, string paramName)
        {
            return self.tmpParamDict.ContainsKey(paramName);
        }
        
        public static bool RemoveParam(this BehaviorMachine self, string paramName)
        {
            if (!self.paramDict.ContainsKey(paramName))
            {
                Log.Error($"does not exist param:{paramName}!");
                return false;
            }

            self.paramDict[paramName].Recycle();
            self.paramDict.Remove(paramName);
            return true;
        }

        public static bool TryRemoveParam(this BehaviorMachine self, string paramName)
        {
            if (!self.paramDict.ContainsKey(paramName))
            {
                return false;
            }

            self.paramDict[paramName].Recycle();
            self.paramDict.Remove(paramName);
            return true;
        }

        public static bool TryRemoveTmpParam(this BehaviorMachine self, string paramName)
        {
            if (!self.tmpParamDict.ContainsKey(paramName))
            {
                return false;
            }

            self.tmpParamDict[paramName].Recycle();
            self.tmpParamDict.Remove(paramName);
            return true;
        }
            
        public static void UpdateParam<T>(this BehaviorMachine self, string paramName, T value)
        {
            foreach ((string key, SharedVariable variable) in self.paramDict)
            {
                if (!key.Equals(paramName))
                {
                    continue;
                }

                variable.value = value;
                return;
            }
            
            Log.Error($"does not exist param:{paramName}!");
        }
        
        #endregion
    }
}