namespace ET.Client
{
    public class Return_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "return";
        }
        
        //return;
        public override async ETTask<Status> Handle(Unit unit, string opCode, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Return;
        }
    }
}