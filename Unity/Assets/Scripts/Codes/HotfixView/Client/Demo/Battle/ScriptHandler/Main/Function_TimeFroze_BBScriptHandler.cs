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
            Match match = Regex.Match(data.opLine, "TimeFroze: (?<Hertz>.*?), (?<HitStop>.*?);");
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
            if (!int.TryParse(match.Groups["HitStop"].Value, out int hitStop))
            {
                Log.Error($"cannot format HitStop to int!");
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            BuffManager buffManager = unit.GetComponent<BuffManager>();

            buffManager.RemoveComponent<TimeFrozeComponent>();
            TimeFrozeComponent timeFroze = buffManager.AddComponent<TimeFrozeComponent>(true);
            timeFroze.Hertz = hertz;
            timeFroze.LastFrame = hitStop;
            timeFroze.unitId = unit.InstanceId;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}