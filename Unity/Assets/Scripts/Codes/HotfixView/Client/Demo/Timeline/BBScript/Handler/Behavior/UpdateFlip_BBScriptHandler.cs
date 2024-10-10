namespace ET.Client
{
    [Invoke(BBTimerInvokeType.UpdateFlipTimer)]
    [FriendOf(typeof(InputWait))]
    [FriendOf(typeof(b2Body))]
    public class UpdateFlipTimer : BBTimer<InputWait>
    {
        protected override void Run(InputWait self)
        {
            //更新刚体朝向
            b2Body b2Body = b2GameManager.Instance.GetBody(self.GetParent<TimelineComponent>().GetParent<Unit>().InstanceId);
            var preFlip = b2Body.Flip;
            var curFlip = preFlip;

            if ((self.Ops & BBOperaType.LEFT) != 0)
            {
                curFlip = FlipState.Left;
            }
            else if ((self.Ops & BBOperaType.RIGHT) != 0)
            {
                curFlip = FlipState.Right;
            }
            else if ((self.Ops & BBOperaType.DOWNLEFT) != 0)
            {
                curFlip = FlipState.Left;
            }
            else if ((self.Ops & BBOperaType.DOWNRIGHT) != 0)
            {
                curFlip = FlipState.Right;
            }

            //转向发生更新
            if (curFlip != preFlip)
            {
                Log.Warning("Update Flip");
                b2Body.SetFlip(curFlip);
                b2Body.SetUpdateFlag();
            }
        }
    }

    public class UpdateFlip_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "UpdateFlip";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBTimerComponent bbTimer = parser.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
            InputWait inputWait = parser.GetParent<TimelineComponent>().GetComponent<InputWait>();
            
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.UpdateFlipTimer, inputWait);
            token.Add(() => { bbTimer.Remove(ref timer); });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}