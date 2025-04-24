using System.Text.RegularExpressions;
using Cinemachine;

namespace ET.Client
{
    [FriendOf(typeof(VirtualCamera))]
    public class Function_Camera_Priority_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Camera_Priority";
        }

        //CM_Priority: DefaultCamera, 1000; 设置虚拟相机的权值
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Camera_Priority: (?<Camera>\w+), (?<Priority>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["Priority"].Value, out int priority))
            {
                Log.Error($"cannot format {match.Groups["Priority"].Value} to int!!!");
                return Status.Failed;
            }

            VirtualCamera camera = VirtualCameraManager.Instance.GetCamera(match.Groups["Camera"].Value);
            CinemachineVirtualCameraBase cameraBase = camera.gameObject.GetComponent<CinemachineVirtualCameraBase>();
            cameraBase.Priority = priority;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}