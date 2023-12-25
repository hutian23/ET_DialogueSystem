namespace ET.Client
{
    public class Knight_Run : BehaviorHandler
    {
        public override int Check(Unit unit, BehaviorConfig config)
        {
            return 0;
        }

        public override async ETTask Handler(Unit unit, BehaviorConfig config, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
        }
    }
}