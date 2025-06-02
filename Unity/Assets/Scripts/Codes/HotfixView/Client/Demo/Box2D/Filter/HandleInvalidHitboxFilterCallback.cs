namespace ET.Client
{
    [Invoke(FilterType.InvalidHitboxFilter)]
    public class HandleInvalidHitboxFilterCallback : AInvokeHandler<B2FilterCallback, bool>
    {
        public override bool Handle(B2FilterCallback args)
        {
            b2Box boxA = Root.Instance.Get(args.instanceIdA) as b2Box;
            b2Box boxB = Root.Instance.Get(args.instanceIdB) as b2Box;
            
            return boxA.GetBoxType() is not HitboxType.Hit || boxB.GetBoxType() is not HitboxType.Hurt;
        }
    }
}