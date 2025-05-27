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
            enemy.AddComponent<B2Unit>();
            enemy.AddComponent<TimelineComponent>();
            enemy.AddComponent<BBTimerComponent>().IsUnitTimer();
            enemy.AddComponent<BBNumeric>();
            enemy.AddComponent<BehaviorMachine>();
            enemy.AddComponent<ObjectWait>();

            // 添加buff
            BuffManager buffManager = enemy.GetComponent<BuffManager>();
            buffManager.AddComponent<HertzAbility>();
            buffManager.AddComponent<HPAbility, int>(100);
            buffManager.AddComponent<SPAbility, int>(100);
            
            // 添加HP数值事件
            HPAbility hpAbility = buffManager.GetComponent<HPAbility>();
            long instanceId = enemy.InstanceId;
            int functionIndex = parser.ContainFunction("Root", "HPWatcher") ? parser.GetFunctionPointer("Root", "HPWatcher") : -1;
            hpAbility.AddChild<NumericWatcher, long, int, string>(instanceId, functionIndex, "HPWatcher");
            
            // 添加SP
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}