namespace ET.Client
{
    public class Return_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "return";
        }

        //return;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Return;
        }
    }
}