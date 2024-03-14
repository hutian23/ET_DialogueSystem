namespace ET.Client
{
    [FriendOf(typeof (WhiffCancel))]
    public class EnableWhiff_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableWhiff";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            GatlingCancelCor(parser,data).Coroutine();
            await ETTask.CompletedTask;
            return Status.Success;
        }

        private async ETTask GatlingCancelCor(BBParser parser, BBScriptData data)
        {
            DialogueComponent dialogueComponent = parser.GetParent<DialogueComponent>();
            WhiffCancel whiff = dialogueComponent.GetComponent<WhiffCancel>();
            BBTimerComponent timerComponent = dialogueComponent.GetComponent<BBInputComponent>().GetComponent<BBTimerComponent>();
            
            while (true)
            {
                Log.Warning("WhiffCancel Check cor");
                await timerComponent.WaitFrameAsync(whiff.token);
                if (whiff.token.IsCancel()) return;
            }
        }
    }
}