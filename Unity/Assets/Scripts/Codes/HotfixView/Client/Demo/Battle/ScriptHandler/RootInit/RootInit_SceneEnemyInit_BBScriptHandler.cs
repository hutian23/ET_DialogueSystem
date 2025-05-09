using Box2DSharp.Dynamics;

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

            //2.
            enemy.AddComponent<TimelineComponent>();
            enemy.AddComponent<BBTimerComponent>().IsUnitTimer();
            enemy.AddComponent<BBNumeric>();
            enemy.AddComponent<B2Unit>();
            enemy.AddComponent<ObjectWait>();
            enemy.AddComponent<BehaviorMachine>();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}