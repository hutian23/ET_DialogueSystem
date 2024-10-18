using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.InputCheckTimer)]
    public class InputCheckTimer: BBTimer<InputWait>
    {
        protected override void Run(InputWait self)
        {
            BBParser parser = self.GetParent<TimelineComponent>().GetComponent<BBParser>();

            if (!self.CheckInput(parser.GetParam<string>("InputCheck")))
            {
                EventSystem.Instance.Invoke(new CancelBehaviorCallback(){instanceId = parser.InstanceId});
            }
        }
    }

    public class InputCheck_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "InputCheck";
        }

        //InputCheck: 'RunHold';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"InputCheck: '(?<InputType>\w+)'");
            if (!match.Success)
            {
                return Status.Failed;
            }

            InputWait inputWait = parser.GetParent<TimelineComponent>().GetComponent<InputWait>();
            BBTimerComponent bbTimer = inputWait.GetComponent<BBTimerComponent>();

            parser.RegistParam("InputCheck", match.Groups["InputType"].Value);

            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.InputCheckTimer, inputWait);
            token.Add(() => { bbTimer.Remove(ref timer); });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}