namespace ET.Client
{
    public class RemoveFlag_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RemoveFlag";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            SkillBuffer buffer = parser.GetParent<TimelineComponent>().GetComponent<SkillBuffer>();
            buffer.RemoveFlag("RunToIdle");
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}