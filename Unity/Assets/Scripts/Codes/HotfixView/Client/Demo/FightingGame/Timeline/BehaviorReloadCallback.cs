using Timeline;

namespace ET.Client
{
    [Invoke]
    public class BehaviorReloadCallback: AInvokeHandler<BehaviorControllerReloadCallback>
    {
        public override void Handle(BehaviorControllerReloadCallback args)
        {
            BBTimelineComponent component = Root.Instance.Get(args.instanceId) as BBTimelineComponent;
            if (component == null) return;

            CodeLoader.Instance.LoadHotfix();
            EventSystem.Instance.Load();

            Log.Warning(component.InstanceId.ToString());
        }
    }
}