namespace ET.Client
{
    public class VN_ActionNodeHandler: NodeHandler<VN_ActionNode>
    {
        protected override async ETTask<Status> Run(Unit unit, VN_ActionNode node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            await DialogueDispatcherComponent.Instance.ScriptHandles(unit, node.Script, token);
            if (token.IsCancel()) return Status.Failed;
            
            node.children.ForEach(childID =>
            {
                DialogueNode childNode = dialogueComponent.GetNode(childID);
                if (childNode.NeedCheck)
                {
                    int ret = DialogueDispatcherComponent.Instance.Checks(unit, childNode.checkList);
                    if (ret == 0) dialogueComponent.PushNextNode(childNode);
                    return;
                }

                dialogueComponent.PushNextNode(childNode);
            });
            return Status.Success;
        }
    }
}