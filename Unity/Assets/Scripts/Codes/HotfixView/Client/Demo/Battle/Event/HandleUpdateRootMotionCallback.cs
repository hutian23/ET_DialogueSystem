using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2Body))]
    public class HandleUpdateRootMotionCallback : AInvokeHandler<UpdateRootMotionCallback>
    {
        public override void Handle(UpdateRootMotionCallback args)
        {
            //1. 查询组件
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            Unit unit = timelineComponent.GetParent<Unit>();
            BBParser bbParser = unit.GetComponent<BBParser>();
            RootMotionComponent rootMotion = bbParser.GetComponent<RootMotionComponent>();

            //2. RootMotion在PreStep中更新刚体速度
            if (rootMotion == null)
            {
                return;
            }
            rootMotion.SetVel(args.velocity.ToVector2());
        }
    }
}