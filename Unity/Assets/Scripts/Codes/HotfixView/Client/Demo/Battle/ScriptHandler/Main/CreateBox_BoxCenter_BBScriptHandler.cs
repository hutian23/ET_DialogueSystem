using System.Text.RegularExpressions;
using Timeline;
using UnityEngine;

namespace ET.Client
{
    public class CreateBox_BoxCenter_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BoxCenter";
        }

        //BoxCenter: 10000, 10000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"BoxCenter: (?<CenterX>-?\d+), (?<CenterY>-?\d+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["CenterX"].Value, out long centerX) || !long.TryParse(match.Groups["CenterY"].Value, out long centerY))
            {
                Log.Error($"cannot format {match.Groups["CenterX"].Value} or {match.Groups["CenterY"].Value} to long!");
                return Status.Failed;
            }

            BoxInfo info = parser.GetParam<BoxInfo>("BoxInfo");
            info.center = new Vector2(centerX, centerY) / 10000f;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}