using System.Text.RegularExpressions;
using Timeline;
using UnityEngine;

namespace ET.Client
{
    public class CreateBox_BoxSize_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BoxSize";
        }

        //BoxSize: 0, 0;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"BoxSize: (?<SizeX>-?\d+), (?<SizeY>-?\d+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            if (!long.TryParse(match.Groups["SizeX"].Value, out long sizeX) || !long.TryParse(match.Groups["SizeY"].Value, out long sizeY))
            {
                Log.Error($"cannot format {match.Groups["SizeX"].Value} or {match.Groups["SizeY"].Value} to long!");
                return Status.Failed;
            }
            
            BoxInfo info = parser.GetParam<BoxInfo>("BoxInfo");
            info.size = new Vector2(sizeX, sizeY) / 10000f;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}