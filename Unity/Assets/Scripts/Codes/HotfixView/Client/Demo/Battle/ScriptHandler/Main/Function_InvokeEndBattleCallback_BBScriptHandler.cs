namespace ET.Client
{
    public class Function_InvokeEndBattleCallback_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "InvokeEndBattleCallback";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            EndBattleManager.Instance.EndBattleCallback();
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}