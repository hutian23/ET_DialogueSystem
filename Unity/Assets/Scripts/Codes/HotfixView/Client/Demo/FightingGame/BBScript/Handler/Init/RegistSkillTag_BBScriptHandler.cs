namespace ET.Client
{
    public class RegistSkillTag_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistSkillTag";
        }

        //RegistSkillTag tag = Sol_GunFlame;
        public override async ETTask<Status> Handle(Unit unit, string opCode, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}