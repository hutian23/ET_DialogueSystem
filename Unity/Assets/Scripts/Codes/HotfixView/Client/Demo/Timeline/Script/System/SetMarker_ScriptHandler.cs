namespace ET.Client
{
    public class SetMarker_ScriptHandler : ScriptHandler
    {
        public override string GetOpType()
        {
            return "SetMarker";
        }

        //无作用， InitScript时已经记录了marker的指针
        public override async ETTask<Status> Handle(ScriptParser parser, ScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}