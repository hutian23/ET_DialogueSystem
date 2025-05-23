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
            if (boxA == null || boxB == null)
            {
                Log.Error($"b2box has been disposed. b2BoxA.instanceId: {args.InstanceIdA}  b2BoxB.instanceId: {args.InstanceIdB}");
                return false;
            }
            if (boxA.GetBoxType() is HitboxType.Gizmos || boxB.GetBoxType() is HitboxType.Gizmos)
            {
                return false;
            }
            
            // Unit之间不会相互挤开 TODO 实现格斗游戏中 SquashBox 互相推动的效果
            if (boxA.GetBoxType() is HitboxType.Squash && boxB.GetBoxType() is HitboxType.Squash &&
                boxA.GetLayerType() is LayerType.Unit && boxB.GetLayerType() is LayerType.Unit)
            {
                return false;
            }
            
            return true;
        }
    }
}