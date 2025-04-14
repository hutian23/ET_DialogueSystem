namespace ET.Client
{
    [FriendOf(typeof(BBTimerComponent))]
    public class NumericChange_UpdateHertz_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "UpdateHertz";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            B2Unit b2Unit = unit.GetComponent<B2Unit>();
            
            int newValue = (int)parser.GetParam<long>("Numeric_NewValue");
            
            //1. 更新战斗定时器timeScale
            bbTimer.SetHertz(newValue);
            bbTimer.Accumulator = 0;

            //2. 速度随timeScale缩放
            b2Unit.SetHertz(newValue);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}