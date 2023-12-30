using System;

namespace ET.Client
{
    public class DialogueAttribute: BaseAttribute
    {
    }
    
    public interface NodeHandler
    {
        public ETTask<Status> Handle(Unit unit, object node, ETCancellationToken token);
        public Type GetDialogueType();
    }

    [Dialogue]
    public abstract class NodeHandler<Node>: NodeHandler where Node : DialogueNode
    {
        protected abstract ETTask<Status> Run(Unit unit, Node node, ETCancellationToken token);

        public async ETTask<Status> Handle(Unit unit, object node, ETCancellationToken token)
        {
            if (node is not Node dialogueNode)
            {
                Log.Error($"节点类型转换错误: {node.GetType().FullName} to {typeof (Node).Name}");
                return Status.Failed;
            }

            return await Run(unit, dialogueNode, token);
        }

        public Type GetDialogueType()
        {
            return typeof (Node);
        }
    }

    public class ChoiceNodeHandler: NodeHandler<ChoiceNode>
    {
        protected override async ETTask<Status> Run(Unit unit, ChoiceNode node, ETCancellationToken token)
        {
            await TimerComponent.Instance.WaitAsync(300, token);
            if (token.IsCancel()) return Status.Failed;
            Log.Warning("Hello world");
            return Status.Success;
        }
    }
}