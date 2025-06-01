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
            if (_root == null) return;

            foreach (BBScript bbScript in _root.GetComponentsInChildren<BBScript>())
            {
                UnitComponent unitComponent = scene.CurrentScene().GetComponent<UnitComponent>();
                Unit unit = unitComponent.AddChild<Unit, int>(1001);

                //渲染层
                unit.AddComponent<GameObjectComponent>().GameObject = bbScript.gameObject;
                bbScript.instanceId = unit.InstanceId;

                //逻辑层
                unit.AddComponent<BBParser>();
            }

            await ETTask.CompletedTask;
        }
    }
}