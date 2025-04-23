using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;

namespace ET.Client
{
    public class CM_TargetGroup_FOV_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CM_TargetGroup_FOV";
        }

        //CM_TargetGroup_MinFOV: TG_Camera, 10000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CM_TargetGroup_FOV: (?<Camera>\w+), (?<MinFov>.*?), (?<MaxFov>.*?);");
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
            
            //1. 查询相机
            Dictionary<string, GameObject> cameraDict = parser.GetParam<Dictionary<string, GameObject>>("CM_CameraDict");
            if (!cameraDict.TryGetValue(match.Groups["Camera"].Value, out GameObject camera))
            {
                Log.Error($"does not exist virtual camera: {match.Groups["Camera"].Value}!!!");
                return Status.Failed;
            }
            
            //2. 
            CinemachineVirtualCamera cm = camera.GetComponent<CinemachineVirtualCamera>();
            cm.GetCinemachineComponent<CinemachineFramingTransposer>().m_MinimumOrthoSize = fov / 10000f;
            cm.GetCinemachineComponent<CinemachineFramingTransposer>().m_MaximumOrthoSize = fov2 / 10000f;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}