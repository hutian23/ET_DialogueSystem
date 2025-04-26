namespace ET.Client
{
    public class Function_HPAdd_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HPAdd";
        }

        //HPAdd: -10;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}