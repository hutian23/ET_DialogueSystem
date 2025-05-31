namespace ET.Client
{
    [Invoke(FilterType.PushBoxFilter)]
    public class HandlePushBoxFilterCallback: AInvokeHandler<B2FilterCallback, bool>
    {
        public override bool Handle(B2FilterCallback args)
        {
            b2Box boxA = Root.Instance.Get(args.instanceIdA) as b2Box;
            b2Box boxB = Root.Instance.Get(args.instanceIdB) as b2Box;
            
            // Unit可以和Unit重叠 TODO 实现格斗游戏中 SquashBox 互相推动的效果
            return boxA.GetBoxType() is not HitboxType.Squash || 
                    boxB.GetBoxType() is not HitboxType.Squash ||
                    boxA.GetLayerType() is not LayerType.Unit ||
                    boxB.GetLayerType() is not LayerType.Unit;
        }
    }
}