using System.Text.RegularExpressions;
using Cinemachine;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(VirtualCamera))]
    public class Function_Camera_TargetGroupMember_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Camera_TargetGroupMember";
        }

        //CM_TargetGroup_Member: DefaultCamera, 100, 100; (camera, weight, radius)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Camera_TargetGroupMember: (?<Camera>\w+), (?<Weight>.*?), (?<Radius>.*?);");
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
            VirtualCamera camera = VirtualCameraManager.Instance.GetCamera(match.Groups["Camera"].Value);
            CinemachineTargetGroup targetGroup = camera.gameObject.GetComponent<CinemachineTargetGroup>();

            GameObject go = parser.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
            targetGroup.AddMember(go.transform, Weight / 100f, radius / 100f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}