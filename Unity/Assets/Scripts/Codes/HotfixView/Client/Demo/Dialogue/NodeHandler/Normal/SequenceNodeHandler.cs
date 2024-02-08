namespace ET.Client
{
    public class SequenceNodeHandler : NodeHandler<SequenceNode>
    {
        protected override async ETTask<Status> Run(Unit unit, SequenceNode node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            foreach (var targetID in node.children)
            {
                DialogueNode child = dialogueComponent.GetNode(targetID);
                // 找到子节点中第一个符合条件的执行
                if (!child.NeedCheck || DialogueDispatcherComponent.Instance.Checks(unit, child.checkList) == 0)
                {
                    dialogueComponent.PushNextNode(targetID);
                    break;
                }
            }
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}