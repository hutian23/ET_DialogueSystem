namespace ET.Client
{
    public class DashHold_InputHandler: BBInputHandler
    {
        public override string GetInputType()
        {
            return "DashHold";
        }

        public override async ETTask<Status> Handle(Unit unit, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}