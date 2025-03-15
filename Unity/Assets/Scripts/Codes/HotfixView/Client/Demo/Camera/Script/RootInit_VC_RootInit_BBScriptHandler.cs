using Box2DSharp.Dynamics;

namespace ET.Client
{
    public class RootInit_VC_RootInit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_RootInit";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            unit.RemoveComponent<VirtualCamera>();
            unit.RemoveComponent<BBTimerComponent>();
            parser.RemoveComponent<SceneBoxHandler>();
            
            unit.AddComponent<VirtualCamera>();
            unit.AddComponent<BBTimerComponent>().IsUnitTimer();
            parser.AddComponent<SceneBoxHandler>();
            b2WorldManager.Instance.CreateBody(unit.InstanceId, new BodyDef() { BodyType = BodyType.StaticBody });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}