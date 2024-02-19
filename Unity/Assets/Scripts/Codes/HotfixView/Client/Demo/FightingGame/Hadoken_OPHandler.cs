namespace ET.Client
{
    [ObjectSystem]
    public class Hadoken_OPHandler
    {
        public async ETTask<Status> CheckHandler(Unit unit, ETCancellationToken token)
        {
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
        // 120 SP
        //必杀前置条件检测 + 按键检测
        /* if SP < 60 return Status.Failed;
         * FightingComponent fightingComponent = unit.GetComponent<FightingComponent>(token);
         * wait = fightingComponent.GetComponent<ObjectWait>().Wait<Next_Input>();
         *
         * if(wait.OpType != OperaType.Up) return failed;
         */
    }
}