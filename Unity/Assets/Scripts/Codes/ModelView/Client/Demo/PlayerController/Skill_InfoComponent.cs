namespace ET.Client
{
    public enum AttackType
    {
        NONE,
        PUNCH,
        KICK,
        SkILL
    }
    
    [ComponentOf(typeof(Unit))]
    public class Skill_InfoComponent : Entity,IAwake,IDestroy
    {
        public int SkillType; // 记录当前技能Id 

        public AttackType AttackType;

        public bool CanExit; // 当前攻击可以被派生取消
    }
}