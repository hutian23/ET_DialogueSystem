namespace ET.Client
{
    [Invoke(BBTimerInvokeType.Test1)]
    [FriendOf(typeof(BBTimerComponent))]
    public class BBTimeTest : BBTimer<BBTimerComponent>
    {
        protected override void Run(BBTimerComponent timerComponent)
        {
            Log.Warning($"{timerComponent.curFrame} {timerComponent.deltaTimereminder}");
        }
    }
}
