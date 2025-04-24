namespace ET.Client
{
    public class Function_Dispose_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Dispose";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            parser.GetParent<Unit>().Dispose();
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}