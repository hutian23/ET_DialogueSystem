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
            BBTimerComponent bbTimer = BBInputHelper.GetBBTimer(unit);

            InputBuffer buffer = InputBuffer.Create(this, bbTimer.GetNow(), bbTimer.GetNow() + 3);
            bool Hold = false;
            while (true)
            {
                //涉及转向
                WaitInput wait = await inputWait.Wait(OP: BBOperaType.DOWN, FuzzyInputType.OR);
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