namespace ET.Client
{
    public class BBNodeHandler : NodeHandler<BBNode>
    {
        protected override async ETTask<Status> Run(Unit unit, BBNode node, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}