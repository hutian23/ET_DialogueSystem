namespace ET.Client
{
    public class RunHold_InputHandler: BBInputHandler
    {
        public override string GetInputType()
        {
            return "RunHold";
        }

        //关于需要持续按住按键的操作可以参考如下
        public override async ETTask<Status> Handle(Unit unit, ETCancellationToken token)
        {
            InputWait inputWait = BBInputHelper.GetInputWait(unit);
            WaitInput wait = await inputWait.Wait(OP: BBOperaType.LEFT | BBOperaType.RIGHT, FuzzyInputType.OR);
            if (wait.Error is not WaitTypeError.Success)
            {
                return Status.Failed;
            }

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}