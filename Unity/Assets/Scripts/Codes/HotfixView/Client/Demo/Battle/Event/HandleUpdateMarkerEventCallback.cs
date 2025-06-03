using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BBParser))]

    [FriendOf(typeof(MarkerEvent))]
    public class HandleUpdateMarkerEventCallback : AInvokeHandler<UpdateEventTrackCallback>
    {
        public override void Handle(UpdateEventTrackCallback args)
        {
            if (Root.Instance.Get(args.instanceId) is not TimelineComponent timelineComponent || timelineComponent.IsDisposed)
            {
                Log.Error($"cannot find timeline component: {args.instanceId}");
                return;
            }

            //1. 查询组件
            Unit unit = timelineComponent.GetParent<Unit>();
            BBParser bbParser = unit.GetComponent<BBParser>();
            MarkerEventManager eventManager = bbParser.GetComponent<MarkerEventManager>();
            if (eventManager == null) return; 
            MarkerEvent markerEvent = eventManager.TryGetMarkerEvent(args.markerName);
            
            //2. 调用事件
            if (markerEvent != null)
            {
                bbParser.Invoke(markerEvent.functionIndex, bbParser.CancellationToken).Coroutine();
            }
        }
    }
}