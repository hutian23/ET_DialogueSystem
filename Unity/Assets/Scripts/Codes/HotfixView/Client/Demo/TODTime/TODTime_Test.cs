namespace ET.Client
{
    [Invoke(TODTimerInvokeType.Test1)]
    [FriendOf(typeof(BBTimerComponent))]
    public class TODTime_Test : TODTimer<BBTimerComponent>
    {
        protected override void Run(BBTimerComponent timerComponent)
        {
            Log.Warning($"{timerComponent.curFrame} {timerComponent.deltaTimereminder}");
        }
    }
}
