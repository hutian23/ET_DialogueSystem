namespace ET.Client
{
    public class Break_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Break";
        }

        //结束while循环
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBTimerComponent bbTimer = parser.GetParent<Unit>().GetComponent<BBTimerComponent>();

            //1. 销毁loop帧检测定时器
            if (parser.ContainParam("BeginLoop_Timer"))
            {
                long loopTimer = parser.GetParam<long>("BeginLoop_Timer");
                bbTimer.Remove(ref loopTimer);   
            }
            parser.TryRemoveParam("BeginLoop_Timer");
            //2. 取消loop协程
            if (parser.ContainParam("BeginLoop_Token"))
            {
                ETCancellationToken loopToken = parser.GetParam<ETCancellationToken>("BeginLoop_Token");
                loopToken.Cancel();
            }
            parser.TryRemoveParam("BeginLoop_Token");
            parser.TryRemoveParam("BeginLoop_TriggerIndex");

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}