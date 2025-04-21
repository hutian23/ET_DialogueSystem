namespace ET.Client
{
    public class RootInit_EnableAirCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableAirCheck";
        }

        //EnableAirCheck: true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 初始化
            Unit unit = parser.GetParent<Unit>();
            BuffManager buffManager = unit.GetComponent<BuffManager>();
            
            //2. 添加组件
            buffManager.RemoveComponent<AirCheckAbility>();
            buffManager.AddComponent<AirCheckAbility>();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}