namespace ET.Client
{
    public class tHandler: NodeHandler<BBRoot>
    {
        protected override async ETTask<Status> Run(Unit unit, BBRoot node, ETCancellationToken token)
        {
            unit.AddComponent<BBInputComponent>();
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}