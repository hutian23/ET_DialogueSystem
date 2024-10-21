namespace ET.Client
{
    public class JumpPressed_InputHandler: BBInputHandler
    {
        public override string GetInputType()
        {
            return "JumpPressed";
        }

        public override async ETTask<Status> Handle(Unit unit, ETCancellationToken token)
        {
            InputWait inputWait = BBInputHelper.GetInputWait(unit);

            WaitInput wait = await inputWait.Wait(OP: BBOperaType.LIGHTKICK, FuzzyInputType.OR, () =>
            {
                bool WasPressedThisFrame = inputWait.WasPressedThisFrame(BBOperaType.LIGHTKICK);
                return WasPressedThisFrame;
            });
            if (wait.Error != WaitTypeError.Success) return Status.Failed;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}