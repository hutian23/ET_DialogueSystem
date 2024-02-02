namespace ET.Client
{
    public class VN_ChoiceNodeHandler: NodeHandler<VN_ChoiceNode>
    {
        protected override async ETTask<Status> Run(Unit unit, VN_ChoiceNode node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            dialogueComponent.PushNextNode(node.next);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}