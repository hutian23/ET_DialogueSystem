using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;

namespace ET.Client
{
    public class CM_Priority_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CM_Priority";
        }

        //CM_Priority: DefaultCamera, 1000; 设置虚拟相机的权值
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CM_Priority: (?<Camera>\w+), (?<Priority>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["Priority"].Value, out int priority))
            {
                Log.Error($"cannot format {match.Groups["Priority"].Value} to int!!!");
                return Status.Failed;
            }

            //1. 查询相机
            Dictionary<string, GameObject> cameraDict = parser.GetParam<Dictionary<string, GameObject>>("CM_CameraDict");
            if (!cameraDict.TryGetValue(match.Groups["Camera"].Value, out GameObject camera))
            {
                Log.Error($"does not exist virtual camera: {match.Groups["Camera"].Value}!!!");
                return Status.Failed;
            }
            
            //2. 修改权重
            camera.GetComponent<CinemachineVirtualCameraBase>().Priority = priority;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}