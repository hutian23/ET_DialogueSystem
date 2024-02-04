namespace ET.Client
{
    public class Persona_ActionNodeHandler: NodeHandler<Persona_ActionNode>
    {
        protected override async ETTask<Status> Run(Unit unit, Persona_ActionNode node, ETCancellationToken token)
        {
            // await DialogueDispatcherComponent.Instance.ScriptHandles(unit, node.Script, token);
            if (token.IsCancel()) return Status.Failed;

            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            node.children.ForEach(childID =>
            {
                DialogueNode childNode = dialogueComponent.GetNode(childID);
                // if (childNode.NeedCheck)
                // {
                //     int ret = DialogueDispatcherComponent.Instance.Checks(unit, childNode.checkList);
                //     if (ret == 0) dialogueComponent.PushNextNode(childNode);
                //     return;
                // }

                dialogueComponent.PushNextNode(childNode);
            });
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}