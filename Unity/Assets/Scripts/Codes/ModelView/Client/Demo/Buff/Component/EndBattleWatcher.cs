namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class EndBattleWatcher : Entity, IAwake<string, string>, IDestroy
    {
        // 记录 BBAction.instanceId
        public long _instanceId;
    }
}