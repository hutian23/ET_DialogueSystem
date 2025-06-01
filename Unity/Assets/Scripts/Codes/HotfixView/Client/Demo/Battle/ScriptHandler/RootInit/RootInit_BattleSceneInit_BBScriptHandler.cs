namespace ET.Client
{
    public class RootInit_BattleSceneInit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BattleSceneInit";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}