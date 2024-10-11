namespace ET.Client
{
    public class LightPunch_InputHandler: BBInputHandler
    {
        public override string GetInputType()
        {
            return "LightPunch";
        }

        public override async ETTask<Status> Handle(Unit unit, ETCancellationToken token)
        {
            InputWait inputWait = BBInputHelper.GetInputWait(unit);

            WaitInput wait = await inputWait.Wait(OP: BBOperaType.LIGHTPUNCH, FuzzyInputType.OR);
            if (wait.Error != WaitTypeError.Success) return Status.Failed;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}