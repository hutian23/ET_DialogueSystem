using Box2DSharp.Dynamics;

namespace ET.Client
{
    public class RootInit_BulletInit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BulletInit";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            
            //Bullet依赖组件
            unit.AddComponent<TimelineComponent>();
            unit.AddComponent<BBTimerComponent>().IsUnitTimer();
            unit.AddComponent<BBNumeric>();
            unit.AddComponent<BehaviorMachine>();
            unit.AddComponent<b2Body, BodyDef>(new BodyDef()
            { 
                BodyType = BodyType.DynamicBody,
                GravityScale = 0f,
                LinearDamping = 0f,
                AngularDamping = 0f,
                AllowSleep = true,
                FixedRotation = true,
            });
            unit.AddComponent<ObjectWait>();

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}