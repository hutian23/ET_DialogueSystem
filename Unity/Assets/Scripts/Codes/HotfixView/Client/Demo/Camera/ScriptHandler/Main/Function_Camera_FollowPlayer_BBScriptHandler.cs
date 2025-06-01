namespace ET.Client
{
    [FriendOf(typeof(CameraManager))]
    public class Function_Camera_FollowPlayer_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Camera_FollowPlayer";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit player = BBUnitHelper.GetPlayer();

            VirtualCameraManager.Instance.RemoveComponent<FollowComponent>();
            VirtualCameraManager.Instance.AddComponent<FollowComponent, long>(player.InstanceId);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}