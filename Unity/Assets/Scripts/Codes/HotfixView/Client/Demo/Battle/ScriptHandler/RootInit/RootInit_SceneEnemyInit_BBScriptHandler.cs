namespace ET.Client
{
    public class RootInit_SceneEnemyInit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SceneEnemyInit";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit enemy = parser.GetParent<Unit>();

            //1. 显示层设置go层级
            enemy.GetComponent<GameObjectComponent>().GameObject.transform.SetParent(GlobalComponent.Instance.Unit);

            //2. 行为机相关组件
            enemy.AddComponent<TimelineComponent>();
            enemy.AddComponent<BBTimerComponent>().IsUnitTimer();
            enemy.AddComponent<BBNumeric>();
            enemy.AddComponent<B2Unit, long>(enemy.InstanceId);
            enemy.AddComponent<ObjectWait>();
            enemy.AddComponent<BehaviorMachine>();

            //3. 单例管理enemy
            // EnemyManager.Instance.InstanceIds.Add(enemy.InstanceId);

            //4. 热重载，移除行为机相关组件
            token.Add(() =>
            {
                enemy.RemoveComponent<TimelineComponent>();
                enemy.RemoveComponent<BBTimerComponent>();
                enemy.RemoveComponent<BBNumeric>();
                enemy.RemoveComponent<B2Unit>();
                enemy.RemoveComponent<ObjectWait>();
                enemy.RemoveComponent<BehaviorMachine>();
                // EnemyManager.Instance.InstanceIds.Remove(enemy.InstanceId);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}