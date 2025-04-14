using System.Numerics;
using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(B2Unit))]
    public class HandleUpdateRootMotionCallback : AInvokeHandler<UpdateRootMotionCallback>
    {
        public override void Handle(UpdateRootMotionCallback args)
        {
            //1. 查询组件
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            Unit unit = timelineComponent.GetParent<Unit>();
            B2Unit b2Unit = unit.GetComponent<B2Unit>();
            BBParser bbParser = unit.GetComponent<BBParser>();

            //2. 是否使用AnimTrack中的移动数据?
            if (!bbParser.ContainParam("ApplyRootMotion"))
            {
                return;
            }
            
            //3. 因为资源中默认朝向为左,横向速度需要翻转
            b2Unit.SetVelocity(args.velocity.ToVector2() * new Vector2(-1, 1));
        }
    }
}