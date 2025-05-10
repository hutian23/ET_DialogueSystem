namespace ET.Client
{
    public class Function_AccelX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AccelX";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}