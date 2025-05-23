namespace ET.Client
{
    public class Function_FlipReverse_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "FlipReverse";
        }
        
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            b2Body body = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);

            int curFlip = body.GetFlip();
            body.SetFlip(-curFlip);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}