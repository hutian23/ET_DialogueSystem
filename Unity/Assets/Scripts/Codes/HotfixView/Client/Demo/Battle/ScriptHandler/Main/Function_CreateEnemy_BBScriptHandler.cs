namespace ET.Client
{
    public class Function_CreateEnemy_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CreateEnemy";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}