namespace ET.Client
{
    [FriendOf(typeof(BlackBoard))]
    public static class BlackBoardSystem
    {
        public class BlackBoardDestroySystem : DestroySystem<BlackBoard>
        {
            protected override void Destroy(BlackBoard self)
            {
                foreach (var kv in self.ValueDict)
                {
                    kv.Value.Recycle();
                }
                self.ValueDict.Clear();
            }
        }

        public static void RegistValue<T>(this BlackBoard self, string name, T value) where T : struct
        {
            if (self.ValueDict.ContainsKey(name))
            {
                Log.Error($"already exist value: {name}");
                return;
            }

            BBValueBase<T> valueBase = BBValueBase<T>.Create(value);
            valueBase.SetValue(value);
            self.ValueDict.Add(name, valueBase);
        }
        
        public static T GetValue<T>(this BlackBoard self, string name) where T : struct
        {
            if (!self.ValueDict.TryGetValue(name, out BBValue bbValue))
            {
                Log.Error($"cannot found value: {name}");
                return default;
            }
            
            BBValueBase<T> valueBase = (BBValueBase<T>)bbValue;
            return valueBase.GetValue();
        }

        public static void SetValue<T>(this BlackBoard self, string name, T value) where T : struct
        {
            if (!self.ValueDict.TryGetValue(name, out BBValue bbValue))
            {
                Log.Error($"cannot found value: {name}");
                return;
            }

            BBValueBase<T> valueBase = (BBValueBase<T>)bbValue;
            valueBase.SetValue(value);
        }
    }
}