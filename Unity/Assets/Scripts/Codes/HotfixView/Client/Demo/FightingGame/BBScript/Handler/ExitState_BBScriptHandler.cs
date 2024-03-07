namespace ET.Client
{
    public class ExitState_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ExitState";
        }
                
        //ExitState;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //取消当前行为以及所有子携程
            parser.Init();
            await ETTask.CompletedTask;
            return Status.Return;
        }
    }
}