namespace ET.Client
{
    public class StartTimeline_ScriptHandler: ScriptHandler
    {
        public override string GetOpType()
        {
            return "StartTimeline";
        }

        
        public override async ETTask<Status> Handle(Unit unit, ScriptData data, ETCancellationToken token)
        {
            while (true)
            {
            }
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}