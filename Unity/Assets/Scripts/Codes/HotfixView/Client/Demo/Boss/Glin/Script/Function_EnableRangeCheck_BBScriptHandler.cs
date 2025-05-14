using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(InRangeCheckComponent))]
    public class Function_EnableRangeCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableRangeCheck";
        }

        // EnableTargetCheck: true, radius, centerX, centerY;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableRangeCheck: (?<Active>\w+), (?<radius>.*?), (?<centerX>.*?), (?<centerY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            //1. 移除组件
            parser.RemoveComponent<InRangeCheckComponent>();
            if (!match.Groups["Active"].Value.Equals("true")) return Status.Success;
            
            //2. 初始化
            if (!long.TryParse(match.Groups["radius"].Value, out long radius) ||
                !long.TryParse(match.Groups["centerX"].Value, out long centerX) ||
                !long.TryParse(match.Groups["centerY"].Value, out long centerY))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            InRangeCheckComponent inRange = parser.AddComponent<InRangeCheckComponent>(true);
            inRange.radius =  radius / 10000f;
            inRange.center = new Vector2(centerX, centerY) / 10000f;
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}