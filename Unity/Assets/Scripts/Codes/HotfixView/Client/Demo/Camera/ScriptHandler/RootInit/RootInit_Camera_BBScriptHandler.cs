using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(VirtualCamera))]
    [FriendOf(typeof(VirtualCameraManager))]
    public class RootInit_Camera_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Camera";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Camera: (?<Name>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //1. Manager管理相机
            VirtualCamera camera = VirtualCameraManager.Instance.AddChild<VirtualCamera>();
            VirtualCameraManager.Instance.AddCamera(match.Groups["Name"].Value, camera);

            //2. 生成相机Go
            GameObject go = new(match.Groups["Name"].Value);
            camera.gameObject = go;
            go.transform.SetParent(VirtualCameraManager.Instance.Global().transform);

            //3. Go初始化
            CinemachineVirtualCamera vc = go.AddComponent<CinemachineVirtualCamera>();
            vc.AddCinemachineComponent<CinemachineFramingTransposer>().enabled = true;
            vc.Follow = VirtualCameraManager.Instance.Target.transform;

            //4. Extension
            go.AddComponent<CinemachineCameraOffset>();
            go.AddComponent<CinemachineConfiner2D>();

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}