namespace ET.Client
{
    //用于取消到特定的动作
    [ComponentOf(typeof(BBParser))]
    public class TargetComboCancelComponent : Entity, IAwake, IDestroy
    {
        public long timer;
    }
}