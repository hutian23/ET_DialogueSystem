namespace ET.Client
{
    [Invoke(FilterType.BulletHitFilter)]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(BulletCaster))]
    public class HandleBulletHitFilterCallback : AInvokeHandler<B2FilterCallback, bool>
    {
        public override bool Handle(B2FilterCallback args)
        {
            b2Box boxA = Root.Instance.Get(args.instanceIdA) as b2Box;
            b2Box boxB = Root.Instance.Get(args.instanceIdB) as b2Box;

            b2Body bodyA = boxA.GetParent<b2Body>();
            b2Body bodyB = boxB.GetParent<b2Body>();

            // 子弹不会和其施放者碰撞
            return bodyA.GetComponent<BulletCaster>()._instanceId != bodyB.unitId;
        }
    }
}