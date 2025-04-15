using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(MarkerEventComponent))]
    public class HandleUpdateMarkerEventCallback : AInvokeHandler<UpdateEventTrackCallback>
    {
        public override void Handle(UpdateEventTrackCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            if (timelineComponent == null)
            {
                Log.Error($"cannot find timeline component: {args.instanceId}");
                return;
            }

            //1. 查询组件
            Unit unit = timelineComponent.GetParent<Unit>();
            BBParser bbParser = unit.GetComponent<BBParser>();
            MarkerEventComponent _event = bbParser.GetComponent<MarkerEventComponent>();

            //2. 调用事件
            if(_event == null || !_event.markerDict.TryGetValue(args.markerName, out MarkerEvent _markerEvent))
            {
                return;
            }
            bbParser.RegistSubCoroutine(_markerEvent.startIndex, _markerEvent.endIndex, bbParser.CancellationToken).Coroutine();
        }
    }
}