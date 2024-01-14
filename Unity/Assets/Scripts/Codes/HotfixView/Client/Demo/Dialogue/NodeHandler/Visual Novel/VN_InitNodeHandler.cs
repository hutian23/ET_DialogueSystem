namespace ET.Client
{
    public class VN_InitNodeHandler: NodeHandler<VN_InitNode>
    {
        protected override async ETTask<Status> Run(Unit unit, VN_InitNode node, ETCancellationToken token)
        {
            await DialogueDispatcherComponent.Instance.ScriptHandles(unit, node.Script, token);
            if (token.IsCancel()) return Status.Failed;
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            dialogueComponent.PushNextNode(node.nextNode);
            return Status.Success;
        }
    }
}