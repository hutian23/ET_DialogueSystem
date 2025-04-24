using System.Text.RegularExpressions;
using Cinemachine;

namespace ET.Client
{
    [FriendOf(typeof(VirtualCamera))]
    public class Function_Camera_OrthoSize_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Camera_OrthoSize";
        }

        //CM_OrthoSize: DefaultCamera, 10; 正交距离
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Camera_OrthoSize: (?<Camera>\w+), (?<Size>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["Size"].Value, out long size))
            {
                Log.Error($"cannot format {match.Groups["Size"].Value} to long!!!");
                return Status.Failed;
            }
            
            VirtualCamera camera = VirtualCameraManager.Instance.GetCamera(match.Groups["Camera"].Value);

            // 设置正交距离
            CinemachineVirtualCamera vc = camera.gameObject.GetComponent<CinemachineVirtualCamera>();
            vc.m_Lens.OrthographicSize = size / 10000f;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}