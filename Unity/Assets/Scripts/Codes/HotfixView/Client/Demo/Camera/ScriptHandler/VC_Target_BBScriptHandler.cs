using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_Target_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Target";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_Target: (?<Weight>.*?), (?<Radius>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["Weight"].Value, out long weight) ||
                !long.TryParse(match.Groups["Radius"].Value, out long radius))
            {
                Log.Error($"cannot format {match.Groups["Weight"].Value} / {match.Groups["Radius"].Value} to long!!");
                return Status.Failed;
            }

            BBParser _parser = VirtualCameraManager.Instance.GetParent<Unit>().GetComponent<BBParser>();
            ListComponent<CameraTarget> targetGroup = _parser.GetParam<ListComponent<CameraTarget>>("VC_TargetGroup");
            
            CameraTarget _target = new()
            {
                instanceId = parser.GetParent<Unit>().InstanceId,
                weight = weight / 100f,
                radius = radius / 100f
            };
            
            targetGroup.Add(_target);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}