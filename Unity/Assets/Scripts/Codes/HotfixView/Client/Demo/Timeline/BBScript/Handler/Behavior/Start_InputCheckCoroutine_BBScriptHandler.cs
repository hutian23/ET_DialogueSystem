using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Start_InputCheckCoroutine_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Start_InputCheckCoroutine";
        }

        //Start_InputCheckCoroutine: 'RunHold';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Start_InputCheckCoroutine: '(?<InputType>\w+)'");
            if (!match.Success)
            {
                return Status.Failed;
            }

            CheckCoroutine(parser, match.Groups["InputType"].Value, token).Coroutine();
            await ETTask.CompletedTask;
            return Status.Success;
        }

        private static async ETTask CheckCoroutine(BBParser parser, string inputType, ETCancellationToken token)
        {
            InputWait inputWait = parser.GetParent<TimelineComponent>().GetComponent<InputWait>();
            BBTimerComponent bbTimer = parser.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
            SkillBuffer skillBuffer = parser.GetParent<TimelineComponent>().GetComponent<SkillBuffer>();

            while (true)
            {
                //检测退出协程
                if (!inputWait.CheckInput(inputType))
                {
                    parser.Cancel();
                    skillBuffer.SetCurrentOrder(-1);
                    return;
                }

                await bbTimer.WaitFrameAsync(token);
                if (token.IsCancel()) return;
            }
        }
    }
}