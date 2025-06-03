namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class EndBattleManager : Entity, IAwake, IDestroy
    {
        [StaticField]
        public static EndBattleManager Instance;
    }

    public struct EndBattleManagerCallback
    {
    }
}