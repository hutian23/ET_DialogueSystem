using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(PatrolComponent))]
    public class Function_EnablePatrol_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnablePatrol";
        }

        //EnablePatrol: MinX, MaxX;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnablePatrol: (?<minX>.*?), (?<maxX>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["minX"].Value, out long minX) ||
                !long.TryParse(match.Groups["maxX"].Value, out long maxX))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            unit.RemoveComponent<PatrolComponent>();
            
            PatrolComponent patrol = unit.AddComponent<PatrolComponent>(true);
            patrol.minX = minX / 10000f;
            patrol.maxX = maxX / 10000f;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}