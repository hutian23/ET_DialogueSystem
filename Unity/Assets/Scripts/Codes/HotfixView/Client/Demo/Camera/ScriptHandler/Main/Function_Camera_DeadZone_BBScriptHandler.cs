using System.Text.RegularExpressions;
using Cinemachine;

namespace ET.Client
{
    [FriendOf(typeof(VirtualCamera))]
    public class Function_Camera_DeadZone_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Camera_DeadZone";
        }

        //设置摄像机死区
        //Camera_DeadZone: DefaultCamera, 30, 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Camera_DeadZone: (?<Camera>\w+), (?<CenterX>.*?), (?<CenterY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["CenterX"].Value, out int centerX) || !int.TryParse(match.Groups["CenterY"].Value, out int centerY))
            {
                Log.Error($"cannot format {match.Groups["CenterX"].Value} / {match.Groups["CenterY"].Value} to int!!!");
                return Status.Failed;
            }

            VirtualCamera camera = VirtualCameraManager.Instance.GetCamera(match.Groups["Camera"].Value);
            CinemachineVirtualCamera vc = camera.gameObject.GetComponent<CinemachineVirtualCamera>();
            CinemachineFramingTransposer composer = vc.GetCinemachineComponent<CinemachineFramingTransposer>();

            composer.m_DeadZoneWidth = centerX / 100f;
            composer.m_DeadZoneHeight = centerY / 100f;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}