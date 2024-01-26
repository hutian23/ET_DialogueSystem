namespace ET.Client
{
    public class VN_ChoicePanelHandler : NodeHandler<VN_ChoicePanel>
    {
        protected override async ETTask<Status> Run(Unit unit, VN_ChoicePanel node, ETCancellationToken token)
        {
            
            if (token.IsCancel()) return Status.Failed;
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}