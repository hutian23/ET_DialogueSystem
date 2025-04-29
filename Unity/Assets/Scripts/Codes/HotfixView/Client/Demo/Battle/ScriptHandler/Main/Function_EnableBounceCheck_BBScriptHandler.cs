using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_EnableBounceCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableBounceCheck";
        }

        //EnableBounceCheck: offsetX, offsetY, sizeX, sizeY, active;
        //EnableBounceCheck: 10000, 10000, 10000, 10000, true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableBounceCheck: (?<offsetX>.*?),(?<offsetY>.*?), (?<sizeX>.*?), (?<sizeY>.*?), (?<active>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["offsetX"].Value, out long offsetX) || !long.TryParse(match.Groups["offsetY"].Value, out long offsetY) ||
                !long.TryParse(match.Groups["sizeX"].Value, out long sizeX) || !long.TryParse(match.Groups["sizeY"].Value, out long sizeY))
            {
                Log.Error($"cannot format {match.Groups["offsetX"].Value} / {match.Groups["offsetY"].Value} / {match.Groups["sizeX"].Value} / {match.Groups["sizeY"].Value} to long!!!");
                return Status.Failed;
            }

            parser.RemoveComponent<BounceCheckComponent>();

            if (match.Groups["active"].Value.Equals("true"))
            {
                parser.AddComponent<BounceCheckComponent, Vector2, Vector2>(new Vector2(offsetX, offsetY) / 10000f, new Vector2(sizeX, sizeY) / 10000f);   
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}