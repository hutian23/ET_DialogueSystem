using Timeline;

namespace ET.Client
{
    [Invoke]
    public class PreviewBehaviorCallback: AInvokeHandler<PreviewReloadCallback>
    {
        public override void Handle(PreviewReloadCallback args)
        {
            PreviewCor(args).Coroutine();
        }

        private async ETTask PreviewCor(PreviewReloadCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            if (timelineComponent == null) return;

            //hot reload
            CodeLoader.Instance.LoadHotfix();
            EventSystem.Instance.Load();

            //Find GameObject
            TimelinePlayer timelinePlayer = timelineComponent.GetTimelinePlayer();
            BBPlayableGraph BBPlayable = timelinePlayer.BBPlayable;
            //Init timelinePlayer
            timelinePlayer.Dispose();
            timelinePlayer.Init(args.Clip.Timeline);

            //init root
            ScriptParser parser = timelineComponent.GetComponent<ScriptParser>();
            parser.InitScript(BBPlayable.root.MainScript);
            await parser.Invoke("Init");

            //preview 
            // parser.InitScript(args.Clip.Script);
            await parser.Invoke("Main");
        }
    }
}