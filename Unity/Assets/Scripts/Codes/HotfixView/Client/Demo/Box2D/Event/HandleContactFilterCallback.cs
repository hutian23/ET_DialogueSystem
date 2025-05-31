using ET.Event;

namespace ET.Client
{
    [Invoke]
    public class HandleContactFilterCallback : AInvokeHandler<ContactFilterCallback, bool>
    {
        public override bool Handle(ContactFilterCallback args)
        {
            b2Box boxA = Root.Instance.Get(args.InstanceIdA) as b2Box;
            b2Box boxB = Root.Instance.Get(args.InstanceIdB) as b2Box;
            if (boxA == null || boxB == null || boxA.IsDisposed || boxB.IsDisposed)
            {
                Log.Error($"b2box has been disposed. b2BoxA.instanceId: {args.InstanceIdA}  b2BoxB.instanceId: {args.InstanceIdB}");
                return false;
            }
            if (boxA.GetBoxType() is HitboxType.Gizmos || boxB.GetBoxType() is HitboxType.Gizmos)
            {
                return false;
            }
            
            b2Body bodyA = boxA.GetParent<b2Body>();
            b2Body bodyB = boxB.GetParent<b2Body>();

            bool ret = bodyA.CollideCheck(args.InstanceIdA, args.InstanceIdB) &&
                    bodyB.CollideCheck(args.InstanceIdB, args.InstanceIdA) &&
                    boxA.CollideCheck(args.InstanceIdB) &&
                    boxB.CollideCheck(args.InstanceIdA);
            return ret;
        }
    }
}