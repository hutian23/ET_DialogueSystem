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
            b2Body body = parser.GetParent<Unit>().GetComponent<b2Body>();

            int curFlip = body.GetFlip();
            body.SetFlip((FlipState)(-curFlip));
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}