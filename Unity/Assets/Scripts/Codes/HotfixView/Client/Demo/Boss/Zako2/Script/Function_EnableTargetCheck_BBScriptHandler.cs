using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(TargetCheckComponent))]
    public class Function_EnableTargetCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableTargetCheck";
        }

        //EnableTargetCheck: CenterX, CenterY, SizeX, SizeY;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableTargetCheck: (?<CenterX>.*?), (?<CenterY>.*?), (?<SizeX>.*?), (?<SizeY>.*?);");
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

            parser.RemoveComponent<TargetCheckComponent>();
            TargetCheckComponent targetCheck = parser.AddComponent<TargetCheckComponent>(true);
            targetCheck.center = new Vector2(centerX, centerY) / 10000f;
            targetCheck.size = new Vector2(sizeX, sizeY) / 10000f;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}