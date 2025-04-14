using System.Numerics;
using Box2DSharp.Testbed.Unity.Inspection;
using Timeline;

namespace ET.Client
{
    [Invoke]
    public class HandleUpdateTargetBindCallback : AInvokeHandler<UpdateTargetBindCallback>
    {
        public override void Handle(UpdateTargetBindCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            Unit unit = timelineComponent.GetParent<Unit>();
            BBParser bbParser = unit.GetComponent<BBParser>();
            
            //Contain TargetBind Unit
            if (!bbParser.ContainParam($"{args.BindTrack.Name}"))
            {
                return;
            }
            
            //Sync TargetBind info
            long b2BodyId = bbParser.GetParam<long>($"{args.BindTrack.Name}");
            b2Body BodyA = b2WorldManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);
            b2Body BodyB = Root.Instance.Get(b2BodyId) as b2Body;

            //1. Sync Position
            Vector2 BindPos = BodyA.GetPosition() + args.KeyFrame.LocalPosition.ToVector2()* new Vector2(BodyA.GetFlip(), 1);
            BodyB.SetPosition(BindPos);
            //2. Sync Flip(默认朝向为左)
            FlipState curFlip = BodyA.GetFlip() == (int)FlipState.Right? (FlipState)(-(int)args.KeyFrame.Flip) : args.KeyFrame.Flip;
            BodyB.SetFlip(curFlip);
        }
    }
}