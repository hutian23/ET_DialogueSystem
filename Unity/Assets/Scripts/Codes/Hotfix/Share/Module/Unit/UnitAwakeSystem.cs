namespace ET
{
    [FriendOf(typeof(Unit))]
    public static class UnitSystem
    {
        [ObjectSystem]
        public class UnitAwakeSystem : AwakeSystem<Unit, int>
        {
            protected override void Awake(Unit self, int filterType)
            {
                self.ConfigId = filterType;
            }
        }

        public class UnitAwake2System : AwakeSystem<Unit, int, UnitType>
        {
            protected override void Awake(Unit self, int configId, UnitType unitType)
            {
                self.ConfigId = configId;
                self.unitType = unitType;
            }
        }

        public static UnitType GetUnitType(this Unit self)
        {
            return self.unitType;
        }
    }
}