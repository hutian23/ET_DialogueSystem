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
            B2Unit b2Unit = parser.GetParent<Unit>().GetComponent<B2Unit>();
            b2Body body = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}