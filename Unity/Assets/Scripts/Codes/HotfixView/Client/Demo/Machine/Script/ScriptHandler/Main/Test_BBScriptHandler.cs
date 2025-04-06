namespace ET.Client
{
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await parser.GetParent<Unit>().GetComponent<BBTimerComponent>().WaitAsync(3, token);
            if (token.IsCancel())
            {
                return Status.Failed;
            }
            
            Log.Warning("HelloWorld");
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}