namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BBAction))]
    [FriendOf(typeof(BBParser))]
    public class HandleBBActionCallback : AInvokeHandler<BBActionCallback>
    {
        public override void Handle(BBActionCallback args)
        {
            Entity parent = Root.Instance.Get(args.instanceId);
            if (parent == null || parent.IsDisposed) return;

            foreach (var kv in parent.Children)
            {
                if(kv.Value is not BBAction action) continue;
                
                Unit unit = Root.Instance.Get(action._instanceId) as Unit;
                BBParser parser = unit.GetComponent<BBParser>();
                parser.Invoke(action.functionIndex, parser.CancellationToken).Coroutine();   
            }
        }
    }
}