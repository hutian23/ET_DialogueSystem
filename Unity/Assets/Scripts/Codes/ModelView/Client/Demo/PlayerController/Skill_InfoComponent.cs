namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class Skill_InfoComponent : Entity,IAwake,IDestroy
    {
        public int SkillType; // 记录当前技能Id 
    }
}