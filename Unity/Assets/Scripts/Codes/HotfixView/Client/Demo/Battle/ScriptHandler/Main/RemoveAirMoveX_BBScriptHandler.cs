namespace ET.Client
{
    public class RemoveAirMoveX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RemoveAirMoveX";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            
            parser.TryRemoveParam("AirMoveX_InertiaEffect");
            parser.TryRemoveParam("AirMoveX_Vel");
            if (parser.ContainParam("AirMoveX_Timer"))
            {
                long preTimer = parser.GetParam<long>("AirMoveX_Timer");
                bbTimer.Remove(ref preTimer);
            }
            parser.TryRemoveParam("AirMoveX_Timer");
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}