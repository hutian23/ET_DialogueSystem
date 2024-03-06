namespace ET.Client
{
    public class ExitState_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ExitState";
        }

        public override async ETTask<Status> Handle(Unit unit, string opCode, ETCancellationToken token)
        {
            //取消当前行为以及所有子携程
            unit.GetComponent<DialogueComponent>().GetComponent<BBParser>().Init();
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}