namespace ET.Client
{
    [FriendOf(typeof (Skill_InfoComponent))]
    public static class Skill_InfoComponentSystem
    {
        public class Skill_InfoComponentDestroySystem: DestroySystem<Skill_InfoComponent>
        {
            protected override void Destroy(Skill_InfoComponent self)
            {
                self.SkillType = 0;
            }
        }

        public static bool SkillHasDisposed(this Unit player)
        {
            if (player == null || player.IsDisposed)
            {
                return true;
            }

            if (player.GetComponent<Skill_InfoComponent>() == null)
            {
                Log.Error($"Please add Skill_InfoComponent to Unit:{player.InstanceId}");
                return true;
            }

            return player.GetComponent<Skill_InfoComponent>().SkillType <= 0;
        }

        public static Skill_InfoComponent Release_Skill(this Unit player, int skillType)
        {
            if (player == null || player.IsDisposed)
            {
                return null;
            }

            Skill_InfoComponent skillInfo = player.GetComponent<Skill_InfoComponent>();
            if (skillInfo == null)
            {
                Log.Error($"Please add Skill_InfoComponent to Unit:{player.InstanceId}");
                return null;
            }

            skillInfo.SkillType = skillType;
            return skillInfo;
        }

        public static void DisposeSkill(this Unit player)
        {
            if (player == null || player.IsDisposed)
            {
                return;
            }

            Skill_InfoComponent skillInfo = player.GetComponent<Skill_InfoComponent>();
            if (skillInfo == null) return;

            skillInfo.SkillType = 0;
            skillInfo.AttackType = 0;
            skillInfo.CanExit = true;
        }

        public static bool ReleasingSkill(this Unit unit, int skillType)
        {
            if (skillType <= 0)
            {
                Log.Warning("compare skillType must be more than 1");
                return false;
            }

            if (unit == null || unit.IsDisposed)
            {
                return true;
            }

            if (unit.GetComponent<Skill_InfoComponent>() == null)
            {
                Log.Error($"please add skill_infoComponent to Unit : {unit.InstanceId}");
                return false;
            }

            return unit.GetComponent<Skill_InfoComponent>().SkillType == skillType;
        }

        public static Skill_InfoComponent SetAttackType(this Unit unit, AttackType attackType)
        {
            if (unit == null || unit.IsDisposed)
            {
                return null;
            }

            Skill_InfoComponent skillInfo = unit.GetComponent<Skill_InfoComponent>();
            if (skillInfo == null)
            {
                Log.Warning("please add skillinfocomponent to unit");
                return null;
            }

            skillInfo.AttackType = attackType;
            return skillInfo;
        }

        public static Skill_InfoComponent SetSkillCanExit(this Unit unit, bool CanExit)
        {
            if (unit == null || unit.IsDisposed)
            {
                return null;
            }

            Skill_InfoComponent skillInfo = unit.GetComponent<Skill_InfoComponent>();
            if (skillInfo == null)
            {
                Log.Warning("please add skillInfoComponent to unit");
                return null;
            }

            skillInfo.CanExit = CanExit;
            return skillInfo;
        }

        public static bool CheckSkillCanExit(this Unit unit)
        {
            return unit?.GetComponent<Skill_InfoComponent>()?.CanExit == true;
        }
        
    }
}