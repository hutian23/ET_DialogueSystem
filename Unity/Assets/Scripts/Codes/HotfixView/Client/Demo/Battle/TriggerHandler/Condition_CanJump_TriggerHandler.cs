using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Condition_CanJump_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "CanJump";
        }

        //CanJump: true;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"CanJump: (?<CanJump>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            Unit unit = parser.GetParent<Unit>();
            JumpAbility ability = unit.GetComponent<BuffManager>().GetComponent<JumpAbility>();
            
            //1. 未查询到组件，认为不能进行跳跃
            if (ability == null) return false;
            
            //TODO 加一个禁止跳跃的Buff or GameTag
            
            //2. 检查跳跃次数
            return match.Groups["CanJump"].Value.Equals("true")? ability.GetJumpCount() > 0 : ability.GetJumpCount() <= 0;
        }
    }
}