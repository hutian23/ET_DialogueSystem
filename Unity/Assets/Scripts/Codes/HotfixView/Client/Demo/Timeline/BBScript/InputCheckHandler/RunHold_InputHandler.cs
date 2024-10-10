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
            BBTimerComponent bbTimer = BBInputHelper.GetBBTimer(unit);

            InputBuffer buffer = InputBuffer.Create(this, bbTimer.GetNow(), bbTimer.GetNow() + 7);
            bool Hold = false;
            while (true)
            {
                //涉及转向
                WaitInput wait = await inputWait.Wait(OP: BBOperaType.LEFT| BBOperaType.RIGHT, FuzzyInputType.OR);
                if (wait.Error is WaitTypeError.Success && !Hold)
                {
                    inputWait.AddBuffer(buffer);
                    Hold = true;
                }
                else
                {
                    return Status.Failed;
                }

                await TimerComponent.Instance.WaitFrameAsync(token);
                if (token.IsCancel()) return Status.Failed;
            }
        }
    }
}