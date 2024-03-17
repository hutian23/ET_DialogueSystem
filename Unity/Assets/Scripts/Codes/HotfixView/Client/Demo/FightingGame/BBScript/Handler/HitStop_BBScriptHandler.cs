using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBTimerComponent))]
    public class HitStop_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitStop";
        }

        // HitStop: 60;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "HitStop: (?<Frame>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            int.TryParse(match.Groups["Frame"].Value, out int frameCount);
            HitStopCor(parser, frameCount, token).Coroutine();

            await ETTask.CompletedTask;
            return token.IsCancel()? Status.Failed : Status.Success;
        }

        private async ETTask HitStopCor(BBParser parser, int frame, ETCancellationToken token)
        {
            BBTimerComponent bbTimer = parser.GetParent<DialogueComponent>().GetComponent<BBInputComponent>().GetComponent<BBTimerComponent>();
            BBTimerComponent combatTimer = parser.GetParent<DialogueComponent>().GetComponent<BBTimerComponent>();

            float timeScale = combatTimer.timeScale;
            combatTimer.timeScale = 0;
            await bbTimer.WaitTillAsync(bbTimer.GetNow() + frame, token);
            if (token.IsCancel()) return;

            combatTimer.timeScale = timeScale;
        }
    }
}