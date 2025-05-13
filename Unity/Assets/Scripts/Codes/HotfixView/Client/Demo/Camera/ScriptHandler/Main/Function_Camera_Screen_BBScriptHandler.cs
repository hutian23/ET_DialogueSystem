using System.Text.RegularExpressions;
using Cinemachine;

namespace ET.Client
{
    [FriendOf(typeof(VirtualCamera))]
    public class Function_Camera_Screen_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Camera_Screen";
        }

        //Camera_Screen: 50, 50;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Camera_Screen: (?<Camera>\w+), (?<ScreenX>.*?), (?<ScreenY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["ScreenX"].Value, out int screenX) || !int.TryParse(match.Groups["ScreenY"].Value, out int screenY))
            {
                Log.Error($"cannot format {match.Groups["ScreenX"].Value} / {match.Groups["ScreenY"].Value} to int!!!");
                return Status.Failed;
            }

            VirtualCamera camera = VirtualCameraManager.Instance.GetCamera(match.Groups["Camera"].Value);
            CinemachineVirtualCamera vc = camera.gameObject.GetComponent<CinemachineVirtualCamera>();
            CinemachineFramingTransposer composer = vc.GetCinemachineComponent<CinemachineFramingTransposer>();

            composer.m_ScreenX = screenX / 100f;
            composer.m_ScreenY = screenY / 100f;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}