namespace ET.Client
{
    [FriendOf(typeof(Skill_InfoComponent))]
    public static class Skill_InfoComponentSystem
    {
        public class Skill_InfoComponentDestroySystem : DestroySystem<Skill_InfoComponent>
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

        public static void ReleaseSkill(this Unit player, int skillType)
        {
            if (player == null || player.IsDisposed)
            {
                return;
            }

            if (player.GetComponent<Skill_InfoComponent>() == null)
            {
                Log.Error($"Please add Skill_InfoComponent to Unit:{player.InstanceId}");
                return;
            }

            player.GetComponent<Skill_InfoComponent>().SkillType = skillType;
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
    }
}