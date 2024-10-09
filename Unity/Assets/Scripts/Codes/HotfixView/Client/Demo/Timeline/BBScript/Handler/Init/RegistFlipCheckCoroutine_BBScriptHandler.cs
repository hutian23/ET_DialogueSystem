namespace ET.Client
{
    public class RegistFlipCheckCoroutine_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistFlipCheck";
        }

        //RegistFlipCheckCoroutine;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Log.Warning("RegistFlipCheck");
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}