namespace ET.Client
{
    public static class EndBattleWatcherSystem
    {
        public class EndBattleWatcherAwakeSystem : AwakeSystem<EndBattleWatcher, string, string>
        {
            protected override void Awake(EndBattleWatcher self, string groupName, string functionName)
            {
                Unit unit = self.GetParent<BuffManager>().GetParent<Unit>();
                BBParser parser = unit.GetComponent<BBParser>();

                // 回调函数的头指针
                int functionIndex = parser.GetFunctionPointer(groupName, functionName);
                
                // 记录bbAction.InstanceId, Dispose时销毁BBAction
                BBAction bbAction = EndBattleManager.Instance.AddChild<BBAction, long, int>(unit.InstanceId, functionIndex);
                self._instanceId = bbAction.InstanceId;
            }
        }
        
        public class EndBattleWatcherDestroySystem : DestroySystem<EndBattleWatcher>
        {
            protected override void Destroy(EndBattleWatcher self)
            {
                if (Root.Instance.Get(self._instanceId) is not BBAction bbAction || bbAction.IsDisposed) return;
                bbAction.Dispose();
            }
        }
    }
}