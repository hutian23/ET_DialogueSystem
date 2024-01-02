namespace ET.Client
{
    public class BubbleBaseNodeHandler : NodeHandler<BubbleBaseNode>
    {
        protected override async ETTask<Status> Run(Unit unit, BubbleBaseNode node, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}