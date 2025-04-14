namespace ET.Client
{ 
    [ComponentOf(typeof(BBParser))]
    public class DefaultCancelComponent: Entity, IAwake, IDestroy
    {
        public long timer;
    }
}