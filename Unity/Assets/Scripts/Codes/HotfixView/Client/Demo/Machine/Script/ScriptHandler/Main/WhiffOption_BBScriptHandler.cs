namespace ET.Client
{
    public class WhiffOption_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "WhiffOption";
        }

        //WhiffOption: Rg_Jump;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}