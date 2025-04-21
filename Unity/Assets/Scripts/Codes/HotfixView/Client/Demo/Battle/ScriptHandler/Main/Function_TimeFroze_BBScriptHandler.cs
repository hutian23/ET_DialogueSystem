using System.Text.RegularExpressions;

namespace ET.Client
{
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
            HertzAbility ability = buffManager.GetComponent<HertzAbility>();
            
            buffManager.RemoveComponent<TimeFrozeComponent>();
            buffManager.AddComponent<TimeFrozeComponent, int, int, long>(hertz, hitStop, ability.InstanceId);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}