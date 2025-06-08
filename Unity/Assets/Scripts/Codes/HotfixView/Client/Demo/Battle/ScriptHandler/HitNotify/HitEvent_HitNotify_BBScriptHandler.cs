using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(HitComponent))]
    public class HitEvent_HitNotify_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitNotify";
        }

        //Once: 在判定框持续持续窗口内，对于同一unit只会产生1hit
        //Repeat: 在判定框持续窗口内，对于同一unit每间隔一frame产生1hit
        //HitNotify: Once, GroupName, FunctionName;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"HitNotify: (?<CheckType>\w+), (?<GroupName>\w+), (?<FunctionName>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            int functionIndex = parser.GetFunctionPointer(match.Groups["GroupName"].Value, match.Groups["FunctionName"].Value);
            string checkType = match.Groups["CheckType"].Value;
            
            //2. 添加攻击检测组件
            parser.RemoveComponent<HitComponent>(); //移除旧的攻击回调
            parser.AddComponent<HitComponent, int, string>(functionIndex, checkType, true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}