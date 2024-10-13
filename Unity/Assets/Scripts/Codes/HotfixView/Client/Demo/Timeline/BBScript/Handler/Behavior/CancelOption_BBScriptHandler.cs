namespace ET.Client
{
    public class CancelOption_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CancelOption";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}