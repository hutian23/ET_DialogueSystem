using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;

namespace ET.Client
{
    public class CM_XDamping_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CM_XDamping";
        }

        //CM_XDamping: DefaultCamera, 100 / 100;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CM_XDamping: (?<Camera>\w+), (?<Damping>.*?);");
            if (!int.TryParse(match.Groups["Damping"].Value, out int damping))
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            //1. 查询相机
            Dictionary<string, GameObject> cameraDict = parser.GetParam<Dictionary<string, GameObject>>("CM_CameraDict");
            if (!cameraDict.TryGetValue(match.Groups["Camera"].Value, out GameObject camera))
            {
                Log.Error($"does not exist virtual camera: {match.Groups["Camera"].Value}!!!");
                return Status.Failed;
            }

            CinemachineVirtualCamera virtualCamera = camera.GetComponent<CinemachineVirtualCamera>();
            CinemachineFramingTransposer transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            if (transposer == null)
            {
                Log.Error($"virtual camera: {match.Groups["Camera"].Value} must set follow to frameComposer!!!");
                return Status.Failed;
            }

            transposer.m_XDamping = damping / 10000f;
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}