namespace ET.Client
{
    public class RemoveDashRecharge_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RemoveDashRecharge";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BehaviorMachine machine = parser.GetParent<Unit>().GetComponent<BehaviorMachine>();
            if (machine.ContainParam("DashRecharge_Token"))
            {
                ETCancellationToken _token = machine.GetParam<ETCancellationToken>("DashRecharge_Token");
                _token.Cancel();
            }
            machine.TryRemoveParam("DashRecharge_Token");

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}