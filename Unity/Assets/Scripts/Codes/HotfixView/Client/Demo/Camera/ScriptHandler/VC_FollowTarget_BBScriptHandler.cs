namespace ET.Client
{
    public class VC_FollowTarget_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_FollowTarget";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}