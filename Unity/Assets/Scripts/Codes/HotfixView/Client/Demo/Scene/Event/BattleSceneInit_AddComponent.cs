using ET.EventType;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class BattleSceneInit_AddComponent : AEvent<BattleSceneInit>
    {
        protected override async ETTask Run(Scene scene, BattleSceneInit args)
        {
            Scene currentScene = scene.CurrentScene();
            
            // 运行时生成的unit(包括Bullet VFX Enemy)全部挂载BattleSceneManager下，热重载时统一销毁
            currentScene.AddComponent<BattleSceneManager>();
            currentScene.AddComponent<BulletManager>();
            
            // 热重载时销毁PoolObject，对Prefab进行更新后热重载销毁旧的实例
            currentScene.AddComponent<PoolManager>();
            
            currentScene.AddComponent<BBInputManager>();            
            currentScene.AddComponent<CameraManager>();
            
            // 逻辑帧
            currentScene.AddComponent<BBTimerManager>();
            // 物理帧
            currentScene.AddComponent<b2WorldManager>();
            
            // 场景中挂载了BBScript脚本的GameObject缓存在此单例中
            currentScene.AddComponent<HotReloadManager>();
            
            GameObject _root = GameObject.Find("_Root");
            if (_root == null)
            {
                Log.Error($"cannot found GameObject _Root in currentScene: {scene.Name}");
                return;
            }
            
            //1. 生成Scene Unit
            foreach (BBScript bbScript in _root.GetComponentsInChildren<BBScript>())
            {
                Unit unit = BattleSceneManager.Instance.AddChild<Unit, int, UnitType>(1001, UnitType.Monster);

                //渲染层传入unit.instanceId
                unit.AddComponent<GameObjectComponent>().GameObject = bbScript.gameObject;
                bbScript.instanceId = unit.InstanceId;

                //逻辑层
                unit.AddComponent<BBParser>();
            }
            
            await ETTask.CompletedTask;
        }
    }
}