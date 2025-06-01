namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2Body))]
    public class HandleInvincibleFilterCallback : AInvokeHandler<B2FilterCallback, bool>
    {
        public override bool Handle(B2FilterCallback args)
        {
            b2Box boxA = Root.Instance.Get(args.instanceIdA) as b2Box;
            b2Box boxB = Root.Instance.Get(args.instanceIdB) as b2Box;

            b2Body bodyA = boxA.GetParent<b2Body>();
            Unit unitA = Root.Instance.Get(bodyA.unitId) as Unit;
            InvincibleAbility invincible = unitA.GetComponent<BuffManager>().GetComponent<InvincibleAbility>();
            
            // 无敌buff时，HitBox不会和HurtBox碰撞
            return invincible == null || boxA.GetBoxType() is not HitboxType.Hurt || boxB.GetBoxType() is not HitboxType.Hurt;
        }
    }
}