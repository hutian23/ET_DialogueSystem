namespace ET.Client
{
    [ChildOf(typeof(MarkerEventManager))]
    public class MarkerEvent : Entity, IAwake<string, int>, IDestroy
    {
        public string markerName;
        public int functionIndex;
    }
}