using System.Collections.Generic;
using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;

namespace ET.Client
{
    public class CM_TargetGroup_Member_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CM_TargetGroup_Member";
        }

        //CM_TargetGroup_Member: DefaultCamera, 100, 100; (camera, weight, radius)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CM_TargetGroup_Member: (?<Camera>\w+), (?<Weight>.*?), (?<Radius>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["Weight"].Value, out int Weight) || !int.TryParse(match.Groups["Radius"].Value, out int radius))
            {
                Log.Error($"cannot format {match.Groups["CenterX"].Value} / {match.Groups["CenterY"].Value} to int!!!");
                return Status.Failed;
            }
            
            //1. 查询相机
            BBParser _parser = VirtualCameraManager.Instance.GetParent<Unit>().GetComponent<BBParser>();
            Dictionary<string, GameObject> cameraDict = _parser.GetParam<Dictionary<string, GameObject>>("CM_CameraDict");
            if (!cameraDict.TryGetValue(match.Groups["Camera"].Value, out GameObject camera))
            {
                Log.Error($"does not exist virtual camera: {match.Groups["Camera"].Value}!!!");
                return Status.Failed;
            }

            CinemachineVirtualCamera cm = camera.GetComponent<CinemachineVirtualCamera>();
            CinemachineTargetGroup targetGroup = cm.Follow.GetComponent<CinemachineTargetGroup>();

            GameObject go = parser.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
            targetGroup.AddMember(go.transform, Weight / 100f, radius / 100f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}