using Box2DSharp.Dynamics;

namespace ET.Client
{
    public class RootInit_GlinInit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GlinInit";
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
            boss.AddComponent<b2Body, BodyDef>(new BodyDef()
            { 
                BodyType = BodyType.DynamicBody,
                GravityScale = 0f,
                LinearDamping = 0f,
                AngularDamping = 0f,
                AllowSleep = true,
                FixedRotation = true,
            });
            boss.AddComponent<ObjectWait>();

            BuffManager buffManager = boss.GetComponent<BuffManager>();
            buffManager.AddComponent<HertzAbility>();
            buffManager.AddComponent<HPAbility, int>(100);
            buffManager.AddComponent<SPAbility, int>(100);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}