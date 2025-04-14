using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    public class CreateBox_BoxName_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BoxName";
        }

        //BoxName: HitStopTest_1
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"BoxName: (?<BoxName>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            FixtureData _data = parser.GetParam<FixtureData>("FixtureData");
            _data.Name = match.Groups["BoxName"].Value;
            parser.UpdateParam("FixtureData", _data);
            
            BoxInfo info = parser.GetParam<BoxInfo>("BoxInfo");
            info.boxName = match.Groups["BoxName"].Value;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}