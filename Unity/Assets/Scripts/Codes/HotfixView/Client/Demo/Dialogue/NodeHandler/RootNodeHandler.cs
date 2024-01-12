namespace ET.Client
{
    public class RootNodeHandler: NodeHandler<RootNode>
    {
        protected override async ETTask<Status> Run(Unit unit, RootNode node, ETCancellationToken token)
        {
            //测试token取消
            // for (int i = 0; i < 10; i++)
            // {
            //     await TimerComponent.Instance.WaitAsync(500, token);
            //     if (token.IsCancel()) return Status.Failed;
            //     Log.Warning($"root_{i}");
            // }

            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            dialogueComponent.PushNextNode(node.nextNode);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}