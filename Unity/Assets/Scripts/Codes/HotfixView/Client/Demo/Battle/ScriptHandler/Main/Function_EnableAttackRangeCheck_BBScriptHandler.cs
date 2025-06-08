using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_EnableAttackRangeCheck_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableAttackRangeCheck";
        }

        //EnableAttackRangeCheck: true, boxCenter.x, boxCenter.Y, boxSize.X, boxSize.Y;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableAttackRangeCheck: (?<Active>\w+), (?<CenterX>.*?), (?<CenterY>.*?), (?<SizeX>.*?), (?<SizeY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["CenterX"].Value, out long centerX) ||
                !long.TryParse(match.Groups["CenterY"].Value, out long centerY) ||
                !long.TryParse(match.Groups["SizeX"].Value, out long sizeX) ||
                !long.TryParse(match.Groups["SizeY"].Value, out long sizeY))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            parser.RemoveComponent<AttackRangeComponent>();

            if (!match.Groups["Active"].Value.Equals("true")) return Status.Success;

            Vector2 center = new Vector2(centerX, centerY) / 10000f;
            Vector2 size = new Vector2(sizeX, sizeY) / 10000f;
            parser.AddComponent<AttackRangeComponent>(true).Init(center, size);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}