using System.Text.RegularExpressions;
using Cinemachine;

namespace ET.Client
{
    [FriendOf(typeof(VirtualCamera))]
    public class Function_Camera_FOV_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Camera_FOV";
        }

        //CM_TargetGroup_MinFOV: TG_Camera, 10000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Camera_FOV: (?<Camera>\w+), (?<MinFov>.*?), (?<MaxFov>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["MinFov"].Value, out long fov) || !long.TryParse(match.Groups["MaxFov"].Value, out long fov2))
            {
                Log.Error($"cannot format {match.Groups["Fov"].Value} to long!");
                return Status.Failed;
            }
            
            VirtualCamera camera = VirtualCameraManager.Instance.GetCamera(match.Groups["Camera"].Value);
            CinemachineVirtualCamera vc = camera.gameObject.GetComponent<CinemachineVirtualCamera>();
            CinemachineFramingTransposer transposer = vc.GetCinemachineComponent<CinemachineFramingTransposer>();

            transposer.m_MinimumOrthoSize = fov / 10000f;
            transposer.m_MaximumOrthoSize = fov2 / 10000f;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}