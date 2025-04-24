using System.Text.RegularExpressions;
using Cinemachine;

namespace ET.Client
{
    [FriendOf(typeof(VirtualCamera))]
    public class Function_Camera_YDamping_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Camera_YDamping";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Camera_YDamping: (?<Camera>\w+), (?<Damping>.*?);");
            if (!int.TryParse(match.Groups["Damping"].Value, out int damping))
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            VirtualCamera camera = VirtualCameraManager.Instance.GetCamera(match.Groups["Camera"].Value);
            CinemachineVirtualCamera vc = camera.gameObject.GetComponent<CinemachineVirtualCamera>();
            CinemachineFramingTransposer transposer = vc.GetCinemachineComponent<CinemachineFramingTransposer>();

            transposer.m_YDamping = damping / 10000f;
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}