namespace ET.Client
{
    public class OpenInputWindow_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "OpenInputWindow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            InputWait inputWait = parser.GetParent<TimelineComponent>().GetComponent<InputWait>();

            inputWait.SetOpenWindow(true);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}