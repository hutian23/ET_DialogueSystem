namespace ET.Client
{
    [FriendOf(typeof(BehaviorInfo))]
    public static class BehaviorInfoSystem
    {
        public class SkillInfoDestroySystem : DestroySystem<BehaviorInfo>
        {
            protected override void Destroy(BehaviorInfo self)
            {
                self.behaviorName = string.Empty;
                self.behaviorOrder = 0;
                self.moveType = MoveType.None;
                foreach (var kv in self.ParamDict)
                {
                    kv.Value.Recycle();
                }
                self.ParamDict.Clear();
            }
        }

        public static T RegistParam<T>(this BehaviorInfo self, string paramName, T value)
        {
            if (self.ParamDict.ContainsKey(paramName))
            {
                Log.Error($"already contain params: {paramName}");
                return default;
            }

            SharedVariable variable = SharedVariable.Create(paramName, value);
            self.ParamDict.Add(paramName, variable);
            return value;
        }

        public static void UpdateParam<T>(this BehaviorInfo self, string paramName, T value)
        {
            foreach ((string key, SharedVariable variable) in self.ParamDict)
            {
                if (!key.Equals(paramName))
                {
                    continue;
                }

                variable.value = value;
                return;
            }
            
            Log.Error($"does not exist param: {paramName}");
        }
        
        public static T GetParam<T>(this BehaviorInfo self, string paramName)
        {
            if (!self.ParamDict.TryGetValue(paramName, out SharedVariable variable))
            {
                Log.Error($"does not exist param:{paramName}!");
                return default;
            }

            if (variable.value is not T value)
            {
                Log.Error($"cannot format {variable.name} to {typeof (T)}");
                return default;
            }

            return value;
        }

        public static bool ContainParam(this BehaviorInfo self, string paramName)
        {
            return self.ParamDict.ContainsKey(paramName);
        }
        
        public static void RemoveParam(this BehaviorInfo self, string paramName)
        {
            if (!self.ParamDict.ContainsKey(paramName))
            {
                Log.Error($"does not exist param:{paramName}!");
                return;
            }

            self.ParamDict[paramName].Recycle();
            self.ParamDict.Remove(paramName);
        }
        
        public static bool TryRemoveParam(this BehaviorInfo self, string paramName)
        {
            if (!self.ParamDict.ContainsKey(paramName))
            {
                return false;
            }
            
            self.ParamDict[paramName].Recycle();
            self.ParamDict.Remove(paramName);
            return true;
        }
    }
}