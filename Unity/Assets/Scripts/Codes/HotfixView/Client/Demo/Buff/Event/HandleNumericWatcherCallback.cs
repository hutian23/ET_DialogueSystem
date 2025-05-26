namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(NumericWatcher))]
    [FriendOf(typeof(BBParser))]
    public class HandleNumericWatcherCallback : AInvokeHandler<NumericWatcherCallback>
    {
        public override void Handle(NumericWatcherCallback args)
        {
            Entity parent = Root.Instance.Get(args.instanceId);
            if (parent == null) return;

            foreach (var kv in parent.Children)
            {
                if(kv.Value is not NumericWatcher watcher) continue;
                
                Unit unit = Root.Instance.Get(watcher._instanceId) as Unit;
                BBParser parser = unit.GetComponent<BBParser>();
                parser.Invoke(watcher.functionIndex, parser.CancellationToken).Coroutine();   
            }
        }
    }
}