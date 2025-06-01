using ET.EventType;
using UnityEngine;

namespace ET.Client
{
    [Event(SceneType.Client)]
    public class AfterSceneChange_BattleSceneInit : AEvent<BattleSceneInit>
    {
        protected override async ETTask Run(Scene scene, BattleSceneInit args)
        {
            GameObject _root = GameObject.Find("_Root");
            if (_root == null)
            {
                Log.Error($"cannot found GameObject _Root in currentScene: {scene.Name}");
                return;
            }
            
            foreach (BBScript bbScript in _root.GetComponentsInChildren<BBScript>())
            {
                Unit unit = BattleSceneManager.Instance.AddChild<Unit, int>(1001);

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