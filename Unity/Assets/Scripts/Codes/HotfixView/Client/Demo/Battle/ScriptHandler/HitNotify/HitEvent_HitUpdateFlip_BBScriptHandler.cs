using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class HitEvent_HitUpdateFlip_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitUpdateFlip";
        }

        // HitUpdateFlip; 受击者面向攻击者
        // 对于一些处决动画, 并不希望受攻击之后更新朝向
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            CollisionBuffer buffer = parser.GetComponent<HitComponent>().GetBuffer();
            b2Box boxA = Root.Instance.Get(buffer.instanceIdA) as b2Box;
            b2Box boxB = Root.Instance.Get(buffer.instanceIdB) as b2Box;
            b2Body bodyA = boxA.GetParent<b2Body>();
            b2Body bodyB = boxB.GetParent<b2Body>();
            
            bodyB.SetFlip((FlipState)(-bodyA.GetFlip()));
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}