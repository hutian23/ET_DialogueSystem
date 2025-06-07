using UnityEngine;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(HotReloadManager))]
    public class HandleHotReloadInitCallback : AInvokeHandler<HotReloadInitCallback>
    {
        public override void Handle(HotReloadInitCallback args)
        {
            GameObject _root = GameObject.Find("_Root");
            if (_root == null)
            {
                Log.Error($"cannot found GameObject _Root in currentScene: {HotReloadManager.Instance.GetParent<Scene>().Name}");
                return;
            }
            
            //1. 查询所有挂载BBScript的对象
            HotReloadManager.Instance.BBScriptQueue.Clear();
            foreach (BBScript bbScript in _root.GetComponentsInChildren<BBScript>())
            {
                HotReloadManager.Instance.BBScriptQueue.Enqueue(bbScript);
                bbScript.gameObject.AddComponent<SceneObject>(); // 热重载时该对象不会销毁
            }

            //2. 生成unit，挂载在BattleSceneManager下
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