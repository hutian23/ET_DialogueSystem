namespace ET.Client
{
    [FriendOf(typeof(BBNumeric))]
    public static class BBNumericSystem
    {
        public static float GetAsFloat(this BBNumeric self, string numericType)
        {
            return (float)self.Get(numericType) / 10000;
        }

        public static int GetAsInt(this BBNumeric self, string numericType)
        {
            return (int)self.Get(numericType);
        }

        public static long GetAsLong(this BBNumeric self, string numericType)
        {
            return self.Get(numericType);
        }
        
        private static long Get(this BBNumeric self, string numericType)
        {
            long value = 0;
            self.NumericDict.TryGetValue(numericType, out value);
            return value;
        }

        public static void Set(this BBNumeric self, string numericType, float value)
        {
            self.Set(numericType, (long)(value * 10000));
        }
        
        public static void Set(this BBNumeric self, string numericType, long value, bool isForce = false)
        {
            //未注册数值，字典添加键值对
            if (self.NumericDict.TryAdd(numericType, value))
            {
                EventSystem.Instance.Invoke(new BBNumericChangedCallback()
                {
                    instanceId = self.InstanceId,
                    numericType = numericType,
                    oldValue = 0,
                    newValue = value
                });
                return;
            }
            
            //更新数值
            long oldValue = self.Get(numericType);
            if (oldValue == value)
            {
                return;
            }
            self.NumericDict[numericType] = value;
            
            //强制刷新数值，不会回调事件
            if (isForce) return;
            EventSystem.Instance.Invoke(new BBNumericChangedCallback()
            {
                instanceId = self.InstanceId,
                numericType = numericType,
                oldValue = oldValue,
                newValue = value
            });
        }
    }
}