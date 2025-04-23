using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(CameraManager))]
    [FriendOf(typeof(BBParser))]
    public class VC_FOVTransition_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_FOVTransition";
        }

        public override async ETTask<Status> Handle(BBParser _parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_FOVTransition: (?<TargetFOV>.*?), (?<Damping>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["TargetFOV"].Value, out long targetFOV) ||
                !long.TryParse(match.Groups["Damping"].Value, out long damping))
            {
                Log.Error($"cannot format {match.Groups["TargetFOV"].Value} / {match.Groups["Damping"].Value} to long!");
                return Status.Failed;
            }

            BBTimerComponent lateUpdateTimer = BBTimerManager.Instance.LateUpdateTimer();
            BBParser parser = VirtualCameraManager.Instance.GetParent<Unit>().GetComponent<BBParser>();

            //1. 初始化
            if (parser.ContainParam("VC_FOV_TransitionTimer"))
            {
                long _timer = parser.GetParam<long>("VC_FOV_TransitionTimer");
                lateUpdateTimer.Remove(ref _timer);
            }
            parser.TryRemoveParam("VC_FOV_TransitionTimer");
            parser.TryRemoveParam("VC_FOV_Damping");
            parser.TryRemoveParam("VC_FOV_TargetFOV");

            //2. 注册相关变量
            long timer = lateUpdateTimer.NewFrameTimer(BBTimerInvokeType.CameraFOVTransitionTimer, parser);
            parser.RegistParam("VC_FOV_TransitionTimer", timer);
            parser.RegistParam("VC_FOV_Damping", damping / 10000f);
            parser.RegistParam("VC_FOV_TargetFOV", targetFOV / 10000f);
            parser.CancellationToken.Add(() =>
            {
                lateUpdateTimer.Remove(ref timer);
            });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}