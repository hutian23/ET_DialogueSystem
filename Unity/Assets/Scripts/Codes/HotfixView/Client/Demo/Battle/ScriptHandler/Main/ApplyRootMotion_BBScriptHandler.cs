using System.Text.RegularExpressions;

namespace ET.Client
{
    public class ApplyRootMotion_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ApplyRootMotion";
        }

        //ApplyRootMotion: true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"ApplyRootMotion: (?<Apply>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //1. 初始化
            parser.TryRemoveParam("ApplyRootMotion");
            
            //2. 添加RootMotion标签
            if (match.Groups["Apply"].Value.Equals("true"))
            {
                parser.RegistParam("ApplyRootMotion", true);
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}