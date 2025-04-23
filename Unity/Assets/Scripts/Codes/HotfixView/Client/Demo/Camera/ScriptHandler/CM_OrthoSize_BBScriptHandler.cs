using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;

namespace ET.Client
{
    public class CM_OrthoSize_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CM_OrthoSize";
        }

        //CM_OrthoSize: DefaultCamera, 10; 正交距离
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CM_OrthoSize: (?<Camera>\w+), (?<Size>.*?);");
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
            
            //1. 查询相机
            Dictionary<string, GameObject> cameraDict = parser.GetParam<Dictionary<string, GameObject>>("CM_CameraDict");
            if (!cameraDict.TryGetValue(match.Groups["Camera"].Value, out GameObject camera))
            {
                Log.Error($"does not exist virtual camera: {match.Groups["Camera"].Value}!!!");
                return Status.Failed;
            }

            //2. 设置正交距离
            CinemachineVirtualCamera cm = camera.GetComponent<CinemachineVirtualCamera>();
            if (cm == null)
            {
                Log.Error($"{match.Groups["Camera"].Value} is not a CinemachineVirtualCamera!!!");
                return Status.Failed;
            }
            cm.m_Lens.OrthographicSize = size / 10000f;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}