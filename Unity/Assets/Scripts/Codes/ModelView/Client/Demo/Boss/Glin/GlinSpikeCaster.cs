namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class GlinSpikeCaster : Entity, IAwake, IDestroy
    {
        public int waitFrame;
        public int spawnCount;
        public ETCancellationToken token;
    }
}