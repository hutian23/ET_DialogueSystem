using System.Numerics;
using Box2DSharp.Dynamics;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.GroundCheckTimer)]
    [FriendOf(typeof (b2GameManager))]
    public class GroundCheckTimer: BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            b2Body body = b2GameManager.Instance.GetBody(self.GetParent<TimelineComponent>().GetParent<Unit>().InstanceId);

            //Raycast 
            World world = b2GameManager.Instance.B2World.World;
            world.RayCast(new GroundCheckRayCastCallback(), body.GetPosition(), body.GetPosition() + new Vector2(0, -6f));
        }
    }

    public class RegistGroundCheck_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistGroundCheck";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();

            bbTimer.NewFrameTimer(BBTimerInvokeType.GroundCheckTimer, parser);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}