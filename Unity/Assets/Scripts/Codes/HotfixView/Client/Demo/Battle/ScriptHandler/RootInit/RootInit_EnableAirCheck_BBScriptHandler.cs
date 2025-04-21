using System.Numerics;
using System.Text.RegularExpressions;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    public class RootInit_EnableAirCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableAirCheck";
        }

        //EnableAirCheck: true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"EnableAirCheck: (?<CenterX>-?\d+), (?<CenterY>-?\d+), (?<SizeX>-?\d+), (?<SizeY>-?\d+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["CenterX"].Value, out long centerX) ||
                !long.TryParse(match.Groups["CenterY"].Value, out long centerY) ||
                !long.TryParse(match.Groups["SizeX"].Value, out long sizeX) ||
                !long.TryParse(match.Groups["SizeY"].Value, out long sizeY))
            {
                Log.Error($"cannot format {match.Groups["SizeX"]} / {match.Groups["SizeY"]} / {match.Groups["CenterX"]} / {match.Groups["CenterY"]} to long!! ");
                return Status.Failed;
            }
            
            //2. 初始化
            Unit unit = parser.GetParent<Unit>();
            BuffManager buffManager = unit.GetComponent<BuffManager>();
            
            //4. 添加组件
            buffManager.RemoveComponent<AirCheckAbility>();
            buffManager.AddComponent<AirCheckAbility>();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}