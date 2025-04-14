using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2Body))]
    public class HandleUpdateSubTimelineCallback : AInvokeHandler<UpdateSubTimelineCallback>
    {
        public override void Handle(UpdateSubTimelineCallback args)
        {
            //TODO 
            // TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            // BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            //
            // if (!bbParser.ContainParam($"{args.Track.Name}"))
            // {
            //     return;
            // }
            //
            // long b2BodyId = bbParser.GetParam<long>($"{args.Track.Name}");
            // b2Body BodyB = Root.Instance.Get(b2BodyId) as b2Body;
            // Unit unit = Root.Instance.Get(BodyB.unitId) as Unit;
            //
            // TimelineComponent _timelineComponent = unit.GetComponent<TimelineComponent>();
            // _timelineComponent.Evaluate(args.keyFrame.TimelineFrame);
        }
    }
}