namespace ET.Client
{
    public class SquitHold_InputHandler: BBInputHandler
    {
        public override string GetInputType()
        {
            return "SquitHold";
        }

        public override async ETTask<Status> Handle(Unit unit, ETCancellationToken token)
        {
            InputWait inputWait = BBInputHelper.GetInputWait(unit);
            
            WaitInput wait = await inputWait.Wait(OP: BBOperaType.DOWN | BBOperaType.DOWNLEFT | BBOperaType.DOWNRIGHT, FuzzyInputType.OR);
            if (wait.Error is not WaitTypeError.Success)
            {
                return Status.Failed;
            }
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}