using System.Text.RegularExpressions;

namespace ET.Client
{
    //对于 run AirBone这些行为，需要在行为中实时转向并改变速度
    public class Function_EnableMoveX_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableMoveX";
        }

        //EnableMoveX: 83000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //正则匹配成员变量
            Match match = Regex.Match(data.opLine, @"EnableMoveX: (?<MoveX>.*?), (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["MoveX"].Value, out long moveX))
            {
                Log.Error($"cannot format {match.Groups["MoveX"].Value} to long!!!");
                return Status.Failed;
            }

            parser.RemoveComponent<MoveXComponent>();

            if (match.Groups["Enable"].Value.Equals("true"))
            {
                parser.AddComponent<MoveXComponent, float>(moveX / 10000f, true);
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}