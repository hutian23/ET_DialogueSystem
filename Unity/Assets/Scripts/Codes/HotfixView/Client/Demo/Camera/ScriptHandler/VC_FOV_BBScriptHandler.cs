using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(CameraManager))]
    public class VC_FOV_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_FOV";
        }

        //VC_Zoom: 80000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_FOV: (?<Size>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["Size"].Value, out long size))
            {
                Log.Error($"cannot format {match.Groups["Size"].Value} to long!");
                return Status.Failed;
            }
            
            float minFov = parser.GetParam<float>("VC_MinFOV");
            float maxFov = parser.GetParam<float>("VC_MaxFOV");
            float curFov = size / 10000f;
            if (curFov < minFov || curFov > maxFov)
            {
                Log.Error($"fov must be between {minFov} and {maxFov}, {curFov}");
                return Status.Failed;
            }

            BBParser _parser = VirtualCameraManager.Instance.GetParent<Unit>().GetComponent<BBParser>();
            _parser.TryRemoveParam("VC_CurrentFOV");
            _parser.RegistParam("VC_CurrentFOV", size / 10000f);
            
            Camera.main.orthographicSize = size / 10000f;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}