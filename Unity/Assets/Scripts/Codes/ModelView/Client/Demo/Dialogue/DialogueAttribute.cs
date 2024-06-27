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

        /// <summary>
        /// BBScript中一行代码表示一个子携程
        /// </summary>
        /// <param name="unit">玩家Entity,通过该entity查找对应的数值组件</param>
        /// <param name="node">当前行为树节点</param>
        /// <param name="token">用来取消当前子携程</param>
        /// <returns></returns>
        public async ETTask<Status> Handle(Unit unit, object node, ETCancellationToken token)
        {
            if (node is not Node dialogueNode)
            {
                Log.Error($"节点类型转换错误: {node.GetType().FullName} to {typeof (Node).Name}");
                return Status.Failed;
            }
            return await Run(unit,dialogueNode , token);
        }

        public Type GetDialogueType()
        {
            return typeof (Node);
        }
    }
}