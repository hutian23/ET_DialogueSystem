using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(VirtualCameraManager))]
    public class RootInit_CameraInit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CameraInit";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            
            //1. 添加虚拟相机管理器
            unit.AddComponent<VirtualCameraManager>();

            //2. 生成CameraTarget
            GameObject target = new("_CameraTarget");
            target.transform.SetParent(go.transform);
            target.transform.localPosition = new Vector3(0, 0, -10);
            VirtualCameraManager.Instance.Target = target;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}