using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(AirMoveXComponent))]
    public class Function_EnableAirMoveX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableAirMoveX";
        }

        //EnableAirMove: 水平移动速度, 启动 / 关闭
        //EnableAirMove: MoveX, Enable;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableAirMoveX: (?<MoveX>.*?), (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["MoveX"].Value, out long moveX))
            {
                Log.Error($"cannot format {match.Groups["MoveX"].Value} to long");
                return Status.Failed;
            }

            //1. 初始化
            parser.RemoveComponent<AirMoveXComponent>();

            //2. 启动AirMove协程
            if (match.Groups["Enable"].Value.Equals("true"))
            { 
                parser.AddComponent<AirMoveXComponent, float>(moveX / 10000f, true);
            }
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}