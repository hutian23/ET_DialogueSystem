namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class IdleAnimComponent : Entity, IAwake<int, string>, IDestroy
    {
        public int counter;
        public string behaviorName;
        public ETCancellationToken token;
    }
}