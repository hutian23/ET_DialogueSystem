namespace ET.Client
{
    public static class TODTimeHelper
    {
        public static async ETTask WaitAsync(this Unit unit, long frame, ETCancellationToken token)
        {
            if (unit == null || unit.IsDisposed)
            {
                Log.Warning("unit is null");
                return;
            }
            BBTimerComponent timerComponent = unit.GetComponent<BBTimerComponent>();
            if (timerComponent == null)
            {
                Log.Warning($"please add todtimercomponent to unit: {unit.InstanceId}");
                return;
            }
            await timerComponent.WaitAsync(frame, token);
        }

        public static async ETTask WaitTillAsync(this Unit unit, long tillFrame, ETCancellationToken token)
        {
            if (unit == null || unit.IsDisposed)
            {
                Log.Warning("unit is null");
                return;
            }
            BBTimerComponent timerComponent = unit.GetComponent<BBTimerComponent>();
            if (timerComponent == null)
            {
                Log.Warning($"please add todtimercomponent to unit: {unit.InstanceId}");
                return;
            }
            await timerComponent.WaitTillAsync(tillFrame, token);
        }
    }
}