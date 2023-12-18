namespace ET.Client
{
    public class Hero_Idle : BehaviorHandler
    {
        public override int Check(Unit unit, BehaviorConfig config)
        {
            return 0;
        }

        public override async ETTask Handler(Unit unit, BehaviorConfig config, ETCancellationToken token)
        {
            Log.Warning("Hello world");
            await ETTask.CompletedTask;
        }
    }
}