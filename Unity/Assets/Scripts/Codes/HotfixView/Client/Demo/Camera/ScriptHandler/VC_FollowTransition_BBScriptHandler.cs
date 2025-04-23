using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_FollowTransition_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_FollowTransition";
        }

        //VC_FollowTransition: true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"VC_FollowTransition: (?<Open>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            BBTimerComponent lateUpdateTimer = BBTimerManager.Instance.LateUpdateTimer();
            
            //1. 初始化
            if (parser.ContainParam("VC_FollowTransitionTimer"))
            {
                long _timer = parser.GetParam<long>("VC_FollowTransitionTimer");
                lateUpdateTimer.Remove(ref _timer);
            }
            parser.TryRemoveParam("VC_FollowTransitionTimer");
            parser.TryRemoveParam("VC_FollowTransition_MinVel");
            parser.TryRemoveParam("VC_FollowTransition_Accel");
            parser.TryRemoveParam("VC_FollowTransition_TransVel");
            parser.TryRemoveParam("VC_FollowTransition_MaxVel");
            
            //2. 注册相关变量
            long followTransitionTimer = lateUpdateTimer.NewFrameTimer(BBTimerInvokeType.CameraOffsetTransitionTimer, parser);
            parser.RegistParam("VC_FollowTransitionTimer", followTransitionTimer);
            parser.RegistParam("VC_FollowTransition_MinVel", 5.1f);
            parser.RegistParam("VC_FollowTransition_Accel", 1f);
            parser.RegistParam("VC_FollowTransition_TransVel", 0f);
            parser.RegistParam("VC_FollowTransition_MaxVel", 1f);
            
            //3. 热重载销毁定时器
            token.Add(() =>
            {
                lateUpdateTimer.Remove(ref followTransitionTimer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}