using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(CameraManager))]
    public class VC_FollowPlayer_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_FollowPlayer";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit player = TODUnitHelper.GetPlayer(parser.ClientScene());

            BBParser _parser = VirtualCameraManager.Instance.GetParent<Unit>().GetComponent<BBParser>();
            //1. 初始化变量
            _parser.TryRemoveParam("VC_Follow_Id");
            _parser.RegistParam("VC_Follow_Id", player.InstanceId);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}