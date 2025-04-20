using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(JumpAbility))]
    public class Function_EnableJump_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableJump";
        }

        //EnableJump: 2;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"EnableJump: (?<MaxJump>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["MaxJump"].Value, out int maxJump))
            {
                Log.Error($"cannot format {match.Groups["MaxJump"].Value} to int!!!");
                return Status.Failed;
            }

            //2. 添加组件，表示Unit可以进行跳跃
            Unit unit = parser.GetParent<Unit>();
            BuffManager buffManager = unit.GetComponent<BuffManager>();
            JumpAbility ability = buffManager.AddComponent<JumpAbility>();

            //3. 数值初始化
            ability.JumpCount = maxJump;
            ability.JumpMaxCount = maxJump;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}