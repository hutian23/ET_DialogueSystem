namespace ET.Client
{
    public class LightPunchPressed_InputHandler: BBInputHandler
    {
        public override string GetInputType()
        {
            return "LightPunchPressed";
        }

        public override async ETTask<Status> Handle(Unit unit, ETCancellationToken token)
        {
            InputWait inputWait = BBInputHelper.GetInputWait(unit);

            WaitInput wait = await inputWait.Wait(OP: BBOperaType.LIGHTPUNCH, FuzzyInputType.OR, () =>
            {
                //避免闭包
                bool WasPressedThisFrame = inputWait.WasPressedThisFrame(BBOperaType.LIGHTPUNCH);
                return WasPressedThisFrame;
            });
            if (wait.Error != WaitTypeError.Success) return Status.Failed;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}