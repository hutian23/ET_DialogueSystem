using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_BulletPos_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BulletPos";
        }

        //BulletPos: 10000, 10000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"BulletPos: (?<PosX>.*?), (?<PosY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}