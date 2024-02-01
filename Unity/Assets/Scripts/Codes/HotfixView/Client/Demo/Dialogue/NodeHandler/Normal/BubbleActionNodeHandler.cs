namespace ET.Client
{
    public class BubbleActionNodeHandler: NodeHandler<BubbleActionNode>
    {
        protected override async ETTask<Status> Run(Unit unit, BubbleActionNode node, ETCancellationToken token)
        {
            await TimerComponent.Instance.WaitAsync(300, token);
            if (token.IsCancel()) return Status.Failed;
            Log.Debug("BubbleAction Node");
            return Status.Success;
        }
    }
}