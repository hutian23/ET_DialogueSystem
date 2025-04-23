using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;

namespace ET.Client
{
    public class CM_SoftZone_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CM_SoftZone";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CM_SoftZone: (?<Camera>\w+), (?<CenterX>.*?), (?<CenterY>.*?);");
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
            
            //1. 查询相机
            Dictionary<string, GameObject> cameraDict = parser.GetParam<Dictionary<string, GameObject>>("CM_CameraDict");
            if (!cameraDict.TryGetValue(match.Groups["Camera"].Value, out GameObject camera))
            {
                Log.Error($"does not exist virtual camera: {match.Groups["Camera"].Value}!!!");
                return Status.Failed;
            }

            //2. 
            CinemachineVirtualCamera cm = camera.GetComponent<CinemachineVirtualCamera>();
            CinemachineFramingTransposer composer = cm.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (composer == null)
            {
                Log.Error($"virtual camera: {match.Groups["Camera"].Value} must set aim to composer!!!");
                return Status.Failed;
            }
            composer.m_SoftZoneWidth = centerX / 100f;
            composer.m_SoftZoneHeight = centerY / 100f;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}