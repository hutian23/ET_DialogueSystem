using System.Text.RegularExpressions;
using Cinemachine;

namespace ET.Client
{
    [FriendOf(typeof(VirtualCamera))]
    public class Function_Camera_SoftZone_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Camera_SoftZone";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Camera_SoftZone: (?<Camera>\w+), (?<CenterX>.*?), (?<CenterY>.*?);");
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

            composer.m_SoftZoneWidth = centerX / 100f;
            composer.m_SoftZoneHeight = centerY / 100f;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}