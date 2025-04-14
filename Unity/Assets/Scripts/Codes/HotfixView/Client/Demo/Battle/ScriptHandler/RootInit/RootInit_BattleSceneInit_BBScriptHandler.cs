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
            Unit unit = parser.GetParent<Unit>();

            //1. 初始化
            unit.AddComponent<BulletManager>();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}