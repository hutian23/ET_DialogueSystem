namespace ET.Client
{
    public class RootInit_EnemyInit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnemyInit";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit enemy = parser.GetParent<Unit>();

            enemy.AddComponent<BuffManager>();
            enemy.AddComponent<Transition>();
            enemy.AddComponent<TimelineComponent>();
            enemy.AddComponent<BBTimerComponent>().IsUnitTimer();
            enemy.AddComponent<BBNumeric>();
            enemy.AddComponent<BehaviorMachine>();
            enemy.AddComponent<ObjectWait>();
            enemy.AddComponent<B2Unit>();

            // 添加buff
            BuffManager buffManager = enemy.GetComponent<BuffManager>();
            buffManager.AddComponent<HPAbility, int>(100);
            buffManager.AddComponent<SPAbility, int>(100);
            buffManager.AddComponent<DeathAbility>();
            // 添加SP
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}