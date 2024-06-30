using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof (ScriptParser))]
    public class BehaviorReloadCallback: AInvokeHandler<BehaviorControllerReloadCallback>
    {
        public override void Handle(BehaviorControllerReloadCallback args)
        {
            TimelineComponent component = Root.Instance.Get(args.instanceId) as TimelineComponent;
            if (component == null) return;

            //hot reload
            CodeLoader.Instance.LoadHotfix();
            EventSystem.Instance.Load();

            //find go
            TimelinePlayer timelinePlayer = component
                    .GetParent<Unit>()
                    .GetComponent<GameObjectComponent>().GameObject
                    .GetComponent<TimelinePlayer>().GetComponent<TimelinePlayer>();
            BBPlayableGraph BBPlayable = timelinePlayer.BBPlayable;

            //parse script
            component.RemoveComponent<ScriptParser>();
            ScriptParser parser = component.AddComponent<ScriptParser>();
            parser.InitScript(BBPlayable.root.MainScript);

            Log.Warning(parser.GetMarker("hutian2501").ToString());
            parser.Invoke("Init", null).Coroutine();
        }
    }
}