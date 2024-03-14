namespace ET.Client
{
    public class DisableWhiff_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DisableWhiff";
        }
            
        //DisableWhiff;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            WhiffCancel whiff = parser.GetParent<DialogueComponent>().GetComponent<WhiffCancel>();
            whiff.Init();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}