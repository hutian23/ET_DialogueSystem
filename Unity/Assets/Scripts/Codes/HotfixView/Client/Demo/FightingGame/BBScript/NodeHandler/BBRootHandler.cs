namespace ET.Client
{
    public class BBRootHandler : NodeHandler<BBRoot>
    {
        protected override async ETTask<Status> Run(Unit unit, BBRoot node, ETCancellationToken token)
        {
            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            dialogueComponent.GetComponent<GatlingCancel>().Clear();          
            
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}