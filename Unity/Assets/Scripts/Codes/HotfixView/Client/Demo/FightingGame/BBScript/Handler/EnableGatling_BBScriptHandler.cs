namespace ET.Client
{
    [FriendOf(typeof (GatlingCancel))]
    public class EnableGatling_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableGatling";
        }

        //EnableGatling;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            WhiffCancelCor(parser, data).Coroutine();
            await ETTask.CompletedTask;
            return Status.Success;
        }

        private async ETTask WhiffCancelCor(BBParser parser, BBScriptData data)
        {
            DialogueComponent dialogueComponent = parser.GetParent<DialogueComponent>();
            GatlingCancel gc = dialogueComponent.GetComponent<GatlingCancel>();
            BBTimerComponent timerComponent = dialogueComponent.GetComponent<BBInputComponent>().GetComponent<BBTimerComponent>();

            while (true)
            {
                Log.Warning("GatlingCancel Check cor....");
                await timerComponent.WaitFrameAsync(gc.token);
                if (gc.token.IsCancel()) return;
            }
        }
    }
}