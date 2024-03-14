namespace ET.Client
{
    public class DisableGatling_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DisableGatling";
        }

        //DisableGatling:
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            GatlingCancel gc = parser.GetParent<DialogueComponent>().GetComponent<GatlingCancel>();
            gc.Init();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}