namespace ET.Client
{
    [ChildOf(typeof(TimelineComponent))]
    public class TimelineMarkerEvent : Entity,IAwake,IDestroy
    {
        public string markerName;
        public int startIndex;
        public int endIndex;
    }
}