using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    public class RootInit_CameraInit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CM_Init";
        }
        
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            BBTimerComponent gizmosTimer = b2WorldManager.Instance.GetGizmosTimer();
            
            unit.RemoveComponent<VirtualCameraManager>();
            unit.AddComponent<VirtualCameraManager>();
            GameObject _camera = unit.GetComponent<GameObjectComponent>().GameObject;

            //1. 清空运行时生成的相机go
            for (int i = 0; i < _camera.transform.childCount; i++)
            {
                Transform child = _camera.transform.GetChild(i);
                UnityEngine.Object.Destroy(child.gameObject);
            }
            
            //2. 生成 CameraTarget
            GameObject target = new("_CameraTarget");
            target.transform.SetParent(_camera.transform);
            target.transform.localPosition = new Vector3(0, 0, -10);
            parser.RegistParam("CM_CameraTarget", target);

            //3. 管理生成的虚拟相机
            Dictionary<string, GameObject> cameraDict = new();
            parser.RegistParam("CM_CameraDict", cameraDict);
            
            //?. 编辑器内
            long timer = gizmosTimer.NewFrameTimer(BBTimerInvokeType.CameraGizmosTimer, parser);
            token.Add(() =>
            {
                gizmosTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}