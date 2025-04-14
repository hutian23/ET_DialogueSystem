using ET.Event;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class HitNotify_UpdateFlip_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Hit_UpdateFlip";
        }

        // Hit_UpdateFlip; 受击者面向攻击者
        // 对于一些处决动画, 并不希望受攻击之后更新朝向
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            CollisionInfo info = parser.GetParam<CollisionInfo>("HitNotify_CollisionInfo");
            b2Body bodyA = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            b2Body bodyB = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
            bodyB.SetFlip((FlipState)(-bodyA.GetFlip()));
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}