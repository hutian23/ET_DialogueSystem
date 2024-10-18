namespace ET.Client
{
    [Invoke]
    public class HandlerCancelBehaviorCallback: AInvokeHandler<CancelBehaviorCallback>
    {
        public override void Handle(CancelBehaviorCallback args)
        {
            //1.取消当前行为协程
            BBParser parser = Root.Instance.Get(args.instanceId) as BBParser;
            parser.Cancel();

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            //2. 行为机控制器重新遍历可进入行为
            SkillBuffer buffer = timelineComponent.GetComponent<SkillBuffer>();
            buffer.SetCurrentOrder(-1);
        }
    }
}