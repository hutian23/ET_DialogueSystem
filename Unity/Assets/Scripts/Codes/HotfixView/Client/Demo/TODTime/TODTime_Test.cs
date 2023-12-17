namespace ET.Client
{
    [Invoke(TODTimerInvokeType.Test1)]
    [FriendOf(typeof(TODTimerComponent))]
    public class TODTime_Test : TODTimer<TODTimerComponent>
    {
        protected override void Run(TODTimerComponent timerComponent)
        {
            Log.Warning($"{timerComponent.curFrame} {timerComponent.deltaTimereminder}");
        }
    }
}
