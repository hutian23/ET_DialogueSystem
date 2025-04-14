namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class FlipCheckComponent : Entity, IAwake, IDestroy
    {
        public ETCancellationToken cancelToken;
    }
}