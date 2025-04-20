using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_JumpAdd_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "JumpAdd";
        }

        //JumpAdd: -1;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"JumpAdd: (?<Count>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["Count"].Value, out int count))
            {
                Log.Error($"cannot format {match.Groups["Count"].Value} to int!!!");
                return Status.Failed;
            }

            //2. 设置当前跳跃次数
            Unit unit = parser.GetParent<Unit>();
            BuffManager buffManager = unit.GetComponent<BuffManager>();
            JumpAbility ja = buffManager.AddComponent<JumpAbility>();
            if (ja == null)
            {
                Log.Error("cannot found JumpAbility !!!");
                return Status.Failed;
            }

            int curCount = ja.GetJumpCount();
            ja.SetJumpCount(curCount + count);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}