namespace ET.Client
{
    public class CancelMoveX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CancelMoveX";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            parser.TryRemoveParam("MoveX_Vel");
            if (parser.ContainParam("MoveX_Timer"))
            {
                long _timer = parser.GetParam<long>("MoveX_Timer");
                parser.GetParent<Unit>().GetComponent<BBTimerComponent>().Remove(ref _timer);
            }
            parser.TryRemoveParam("MoveX_Timer");
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}