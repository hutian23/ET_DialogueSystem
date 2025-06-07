namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(HotReloadManager))]
    public class HandleHotReloadCallback : AInvokeHandler<HotReloadCallBack>
    {
        public override void Handle(HotReloadCallBack args)
        {
            int count = HotReloadManager.Instance.BBScriptQueue.Count;
            while (count -- > 0)
            {
                BBScript bbScript = HotReloadManager.Instance.BBScriptQueue.Dequeue();
                HotReloadManager.Instance.BBScriptQueue.Enqueue(bbScript);
                
                Unit unit = BattleSceneManager.Instance.AddChild<Unit, int>(1001);
                
                // 渲染层传入unit.instanceId
                unit.AddComponent<GameObjectComponent>().GameObject = bbScript.gameObject;
                bbScript.instanceId = unit.InstanceId;
                
                // 逻辑层，初始化unit
                unit.AddComponent<BBParser>();
            }
        }
    }
}
