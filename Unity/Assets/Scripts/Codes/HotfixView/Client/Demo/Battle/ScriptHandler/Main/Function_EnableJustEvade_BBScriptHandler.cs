using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(JustEvadeComponent))]
    public class Function_EnableJustEvade_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableJustEvade";
        }

        // EnableJustEvade: 精准闪避窗口持续帧, box.size.x, box.size.y, box.center.x, box.center.y;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableJustEvade: (?<LastFrame>.*?), (?<SizeX>.*?), (?<SizeY>.*?), (?<CenterX>.*?), (?<CenterY>.*?)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["LastFrame"].Value, out int lastFrame) ||
                !long.TryParse(match.Groups["CenterX"].Value, out long centerX) ||
                !long.TryParse(match.Groups["CenterY"].Value, out long centerY) ||
                !long.TryParse(match.Groups["SizeX"].Value, out long sizeX) ||
                !long.TryParse(match.Groups["SizeY"].Value, out long sizeY))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }

            parser.RemoveComponent<JustEvadeComponent>();
            JustEvadeComponent justEvade = parser.AddComponent<JustEvadeComponent>(true);
            justEvade.boxSize = new Vector2(sizeX, sizeY) / 10000f;
            justEvade.boxOffset = new Vector2(centerX, centerY) / 10000f;
            justEvade.lastFrame = lastFrame;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}