using UnityEngine;

namespace ET.Client
{
    public class CM_Follow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CM_Follow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBTimerComponent lateUpdateTimer = BBTimerManager.Instance.LateUpdateTimer();
            BBParser _parser = VirtualCameraManager.Instance.GetParent<Unit>().GetComponent<BBParser>();
            GameObject go = parser.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
            
            //1. 初始化
            if (_parser.ContainParam("CM_Follow_Timer"))
            {
                long _timer = _parser.GetParam<long>("CM_Follow_Timer");
                lateUpdateTimer.Remove(ref _timer);
            }
            _parser.TryRemoveParam("CM_Follow_Timer");
            _parser.TryRemoveParam("CM_Follow_Go");

            //2. 
            long timer = lateUpdateTimer.NewFrameTimer(BBTimerInvokeType.CinemachineTargetTimer, _parser);
            _parser.RegistParam("CM_Follow_Timer", timer);
            _parser.RegistParam("CM_Follow_Go", go);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}