namespace ET.Client
{
    public class RunHold_InputHandler: BBInputHandler
    {
        public override string GetInputType()
        {
            return "RunHold";
        }

        public override async ETTask<Status> Handle(Unit unit, ETCancellationToken token)
        {
            InputWait inputWait = unit.GetComponent<TimelineComponent>().GetComponent<InputWait>();

            WaitInput wait = await inputWait.Wait(OP: BBOperaType.LEFT | BBOperaType.RIGHT, FuzzyInputType.OR);
            if (wait.Error != WaitTypeError.Success) return Status.Failed;
            return Status.Success;
        }
    }
}