namespace ET.Server
{
    public static class UnitGateComponentSystem
    {
        public class UnitGateComponentAwakeSystem : AwakeSystem<UnitGateComponent, long>
        {
            protected override void Awake(UnitGateComponent self, long filterType)
            {
                self.GateSessionActorId = filterType;
            }
        }
    }
}