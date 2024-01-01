namespace ET.Client
{
    public class RootNodeHandler : NodeHandler<RootNode>
    {
        protected override async ETTask<Status> Run(Unit unit, RootNode node, ETCancellationToken token)
        {
            DialogueNode next = node.nextNode;
            while (true)
            {
                if(token.IsCancel()) break;
                Log.Warning("Hello world222");
                await TimerComponent.Instance.WaitFrameAsync(token);
            }
            return Status.Success;
        }
    }
}