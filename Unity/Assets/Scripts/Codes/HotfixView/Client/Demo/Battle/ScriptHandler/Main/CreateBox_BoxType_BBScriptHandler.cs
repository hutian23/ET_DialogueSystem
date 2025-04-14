using System;
using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    public class CreateBox_BoxType_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BoxType";
        }
        
        //BoxType: Squash
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"BoxType: (?<BoxType>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!Enum.TryParse(match.Groups["BoxType"].Value, out HitboxType hitboxType))
            {
                Log.Warning($"cannot format {match.Groups["BoxType"].Value} to hitboxType");
                return Status.Failed;
            }

            BoxInfo info = parser.GetParam<BoxInfo>("BoxInfo");
            info.hitboxType = hitboxType;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}