using System.Text.RegularExpressions;

namespace ET.Client
{
    public class IdleAnim_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "IdleAnim";
        }

        //IdleAnim: Rg_IdleAnim;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "IdleAnim: (?<Animation>.*?), (?<WaitFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["WaitFrame"].Value, out int waitFrame))
            {
                Log.Error($"cannot format {match.Groups["WaitFrame"].Value} to int!!");
                return Status.Failed;
            }
            
            IdleAnimCor(parser.GetParent<Unit>(), match.Groups["Animation"].Value, waitFrame, token).Coroutine();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }

        private async ETTask IdleAnimCor(Unit unit, string behaviorName, int waitFrame, ETCancellationToken token)
        {
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            
            await bbTimer.WaitAsync(waitFrame, token);
            if (token.IsCancel())
            {
                return;
            }
            machine.Reload(behaviorName);
        }
    }
}