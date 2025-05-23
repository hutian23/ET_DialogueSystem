namespace ET.Client
{
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Log.Warning(parser.GetComponent<GroundCollisionComponent>().GetGroundCollision().ToString());
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}