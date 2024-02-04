namespace ET.Client
{
    public class RootNodeHandler: NodeHandler<RootNode>
    {
        protected override async ETTask<Status> Run(Unit unit, RootNode node, ETCancellationToken token)
        {
            await DialogueDispatcherComponent.Instance.ScriptHandles(unit, node, token);
            if (token.IsCancel()) return Status.Failed;

            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            dialogueComponent.PushNextNode(node.nextNode);

            return Status.Success;
        }
    }
}