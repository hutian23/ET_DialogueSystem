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
            Unit boss = parser.GetParent<Unit>();

            boss.AddComponent<BuffManager>();
            boss.AddComponent<Transition>();
            boss.AddComponent<TimelineComponent>();
            boss.AddComponent<BBTimerComponent>().IsUnitTimer();
            boss.AddComponent<BBNumeric>();
            boss.AddComponent<BehaviorMachine>();
            boss.AddComponent<B2Unit>();
            boss.AddComponent<ObjectWait>();

            BuffManager buffManager = boss.GetComponent<BuffManager>();
            buffManager.AddComponent<HertzAbility>();

            HPAbility hpAbility = buffManager.AddComponent<HPAbility, int>(100);
            hpAbility.AddChild<NumericWatcher>();
            buffManager.AddComponent<SPAbility, int>(100);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}