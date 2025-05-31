namespace ET.Client
{
    [Invoke(FilterType.PushBoxFilter)]
    public class HandlePushBoxFilterCallback: AInvokeHandler<B2FilterCallback, bool>
    {
        public override bool Handle(B2FilterCallback args)
        {
            b2Box boxA = Root.Instance.Get(args.instanceIdA) as b2Box;
            b2Box boxB = Root.Instance.Get(args.instanceIdB) as b2Box;
            
            return boxA.GetBoxType() is HitboxType.Squash && 
                    boxB.GetBoxType() is HitboxType.Squash &&
                    boxA.GetLayerType() is LayerType.Unit && 
                    boxB.GetLayerType() is LayerType.Unit;
        }
    }
}