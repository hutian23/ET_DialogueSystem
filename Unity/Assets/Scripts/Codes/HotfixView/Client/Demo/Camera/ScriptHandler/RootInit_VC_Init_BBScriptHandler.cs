using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(CameraManager))]
    public class RootInit_VC_Init_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Init";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBTimerComponent lateUpdateTimer = BBTimerManager.Instance.LateUpdateTimer();
            BBTimerComponent GizmosTimer = b2WorldManager.Instance.GetGizmosTimer();
            
            //1. Camera Follow
            // long followTimer = lateUpdateTimer.NewFrameTimer(BBTimerInvokeType.CameraFollowTimer, parser);
            // parser.RegistParam("VC_Follow_Id", long.MinValue);
            // parser.RegistParam("VC_Follow_TargetPosition", Vector2.zero);
            // parser.RegistParam("VC_Follow_Center", Vector2.zero);
            // parser.RegistParam("VC_Follow_Offset", 0f);
            // parser.RegistParam("VC_Follow_CurrentOffset", 0f);
            // parser.RegistParam("VC_Follow_Timer", followTimer);
            // token.Add(() =>
            // {
            //     lateUpdateTimer.Remove(ref followTimer);
            // });

            //1. Zone
            long zoneTimer = lateUpdateTimer.NewFrameTimer(BBTimerInvokeType.CameraZoneTimer, parser);
            parser.RegistParam("VC_Center", Vector2.zero);
            parser.RegistParam("VC_DeadZone_X", 0f);
            parser.RegistParam("VC_DeadZone_Y", 0f);
            parser.RegistParam("VC_SoftZone_X", 1f);
            parser.RegistParam("VC_SoftZone_Y", 1f);
            parser.RegistParam("VC_SoftZone_Rect", new Rect());
            parser.RegistParam("VC_DeadZone_Rect", new Rect());
            parser.RegistParam("VC_Bias_X", 0.01f);
            parser.RegistParam("VC_Bias_Y", 0.01f);
            parser.RegistParam("VC_ZoneTimer", zoneTimer);
            parser.RegistParam("VC_Damping_X", 5f);
            parser.RegistParam("VC_Damping_Y", 5f);
            token.Add(() =>
            {
                lateUpdateTimer.Remove(ref zoneTimer);
            });

            //2. FOV
            parser.RegistParam("VC_CurrentFOV", 8f);
            parser.RegistParam("VC_MinFOV", 8f);
            parser.RegistParam("VC_MaxFOV", 8f);
            
            //3. Draw Gizmos
            long _GizmosTimer = GizmosTimer.NewFrameTimer(BBTimerInvokeType.CameraGizmosTimer, parser);
            parser.RegistParam("VC_GizmosTimer", GizmosTimer);
            token.Add(() =>
            {
                GizmosTimer.Remove(ref _GizmosTimer);
            });
            
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}