namespace ET.Client
{
    public class AddFlag_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AddFlag";
        }

        //AddFlag: "Run";
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            SkillBuffer buffer = parser.GetParent<TimelineComponent>().GetComponent<SkillBuffer>();
            buffer.AddFlag("RunToIdle");
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}