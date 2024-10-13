namespace ET.Client
{
    public class Dash_InputHandler: BBInputHandler
    {
        public override string GetInputType()
        {
            return "Dash";
        }

        public override async ETTask<Status> Handle(Unit unit, ETCancellationToken token)
        {
            InputWait inputWait = BBInputHelper.GetInputWait(unit);
            
            WaitInput wait = await inputWait.Wait(OP: BBOperaType.MIDDLEKICK, FuzzyInputType.AND, () => inputWait.WasPressedThisFrame(BBOperaType.MIDDLEKICK));
            if (wait.Error != WaitTypeError.Success) return Status.Failed;

            return Status.Success;
        }
    }
}