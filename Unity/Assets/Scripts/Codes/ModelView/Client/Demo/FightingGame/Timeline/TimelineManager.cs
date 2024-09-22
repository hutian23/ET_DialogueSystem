namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class TimelineManager: Entity, IAwake, IDestroy,ILoad
    {
        [StaticField]
        public static TimelineManager Instance;
    }
}