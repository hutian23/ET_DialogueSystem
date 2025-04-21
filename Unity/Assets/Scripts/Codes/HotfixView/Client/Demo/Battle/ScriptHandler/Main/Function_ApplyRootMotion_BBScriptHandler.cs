using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_ApplyRootMotion_BBScriptHandler : BBScriptHandler
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
            parser.RemoveComponent<RootMotionComponent>();
            
            //2. 添加组件，认为正在进行根运动，退出当前行为时，销毁组件
            if (match.Groups["Apply"].Value.Equals("true"))
            {
                parser.AddComponent<RootMotionComponent>(true);
            }

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}