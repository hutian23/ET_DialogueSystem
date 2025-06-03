using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(TimeFrozeComponent))]
    public class Function_TimeFroze_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "TimeFroze";
        }

        //TimeFroze: 0, 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "TimeFroze: (?<LastFrame>.*?), (?<Hertz>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["Hertz"].Value, out int hertz))
            {
                Log.Error($"cannot format Hertz to int!");
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["LastFrame"].Value, out int lastFrame))
            {
                Log.Error($"cannot format HitStop to int!");
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            BuffManager buffManager = unit.GetComponent<BuffManager>();

            buffManager.RemoveComponent<TimeFrozeComponent>();
            buffManager.AddComponent<TimeFrozeComponent, int, int, long>(hertz, lastFrame, unit.InstanceId, true);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}