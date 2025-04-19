namespace ET.Client
{
    public class Function_GroundDashOP_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GroundDashOP";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}