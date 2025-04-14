namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class NandemoCancelComponent : Entity, IAwake, IDestroy
    {
        public long timer;
    }
}