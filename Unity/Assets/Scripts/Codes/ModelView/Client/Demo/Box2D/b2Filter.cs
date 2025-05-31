namespace ET.Client
{
    [ChildOf(typeof(b2Body))]
    public class b2Filter : Entity, IAwake<int>, IDestroy
    {
        public int type;
    }
}