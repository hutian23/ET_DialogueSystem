namespace ET.Client
{
    public class RootNodeHandler: NodeHandler<RootNode>
    {
        protected override async ETTask<Status> Run(Unit unit, RootNode node, ETCancellationToken token)
        {
            token.Add(() => { Log.Warning("携程被取消"); }); //携程被取消的回调
            await TimerComponent.Instance.WaitAsync(3000, token);
            if (token.IsCancel()) return Status.Failed; // 携程被取消，就不往后面执行了
            Log.Warning("Hello world");

            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            dialogueComponent.PushNextNode(node.nextNode);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}