namespace ET.Client
{
    public class RootNodeHandler : NodeHandler<RootNode>
    {
        protected override async ETTask<Status> Run(Unit unit, RootNode node, ETCancellationToken token)
        {
            DialogueNode next = node.nextNode;
            Log.Warning(next.ToString());
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}