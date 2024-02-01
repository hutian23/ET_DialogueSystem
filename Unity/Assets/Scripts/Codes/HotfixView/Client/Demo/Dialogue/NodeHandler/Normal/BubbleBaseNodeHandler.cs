namespace ET.Client
{
    public class BubbleBaseNodeHandler: NodeHandler<BubbleBaseNode>
    {
        protected override async ETTask<Status> Run(Unit unit, BubbleBaseNode node, ETCancellationToken token)
        {
            unit.GetComponent<DialogueComponent>().PushNextNode(node.bubbles);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}