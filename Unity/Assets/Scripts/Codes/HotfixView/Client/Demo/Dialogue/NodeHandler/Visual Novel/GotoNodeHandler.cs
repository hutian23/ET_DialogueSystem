namespace ET.Client
{
    public class GotoNodeHandler: NodeHandler<GotoNode>
    {
        protected override async ETTask<Status> Run(Unit unit, GotoNode node, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            unit.GetComponent<DialogueComponent>().PushNextNode((uint)node.Goto_targetID);
            return Status.Success;
        }
    }
}