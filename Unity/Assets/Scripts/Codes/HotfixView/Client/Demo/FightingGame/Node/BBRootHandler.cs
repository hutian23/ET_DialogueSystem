namespace ET.Client
{
    public class tHandler: NodeHandler<BBRoot>
    {
        protected override async ETTask<Status> Run(Unit unit, BBRoot node, ETCancellationToken token)
        {
            // unit.AddComponent<BBInputComponent>();
            foreach (uint targetID in node.behaviors)
            {
                
            }
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}