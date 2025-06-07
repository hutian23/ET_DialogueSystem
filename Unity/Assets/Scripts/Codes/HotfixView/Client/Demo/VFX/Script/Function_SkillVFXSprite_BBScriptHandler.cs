namespace ET.Client
{
    public class Function_SkillVFXSprite_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SkillVFXSprite";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}