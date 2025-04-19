namespace ET.Client
{
    public class Function_EnableAirDash_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableAirDash";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}