using System.Text.RegularExpressions;

namespace ET.Client
{
    
    [FriendOf(typeof(BBParser))]
    public class Function_HitStop_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitStop";
        }

        //HitStop: 6, 8;(Hertz, hitStopFrame)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "HitStop: (?<Hertz>.*?), (?<HitStop>.*?);");
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
            HertzAbility ability = unit.GetComponent<BuffManager>().GetComponent<HertzAbility>();
            
            parser.RemoveComponent<TimeFrozeComponent>();
            parser.AddComponent<TimeFrozeComponent, int, int, long>(hertz, hitStop, ability.InstanceId);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}