using System.Text.RegularExpressions;
using Cinemachine;

namespace ET.Client
{
    [FriendOf(typeof(VirtualCamera))]
    public class Function_Camera_Bias_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Camera_Bias";
        }

        //Camera_Bias: DefaultCamera, 30, 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Camera_Bias: (?<Camera>\w+), (?<CenterX>.*?), (?<CenterY>.*?);");
            if (!int.TryParse(match.Groups["CenterX"].Value, out int centerX) || !int.TryParse(match.Groups["CenterY"].Value, out int centerY))
            {
                Log.Error($"cannot format {match.Groups["CenterX"].Value} / {match.Groups["CenterY"].Value} to int!!!");
                return Status.Failed;
            }

            VirtualCamera camera = VirtualCameraManager.Instance.GetCamera(match.Groups["Camera"].Value);
            CinemachineVirtualCamera vc = camera.gameObject.GetComponent<CinemachineVirtualCamera>();
            CinemachineFramingTransposer composer = vc.GetCinemachineComponent<CinemachineFramingTransposer>();

            composer.m_BiasX = centerX / 100f;
            composer.m_BiasX = centerY / 100f;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}