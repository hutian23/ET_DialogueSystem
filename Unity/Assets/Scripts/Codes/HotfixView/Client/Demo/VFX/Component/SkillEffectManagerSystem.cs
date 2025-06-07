namespace ET.Client
{
    [FriendOf(typeof(SkillEffectManager))]
    public static class SkillEffectManagerSystem
    {
        public class SkillEffectAwakeSystem : AwakeSystem<SkillEffectManager>
        {
            protected override void Awake(SkillEffectManager self)
            {
                self.effectDict.Clear();
            }
        }

        public class SkillEffectDestroySystem : DestroySystem<SkillEffectManager>
        {
            protected override void Destroy(SkillEffectManager self)
            {
                self.effectDict.Clear();
            }
        }

        public static void RegistSkillEffect(this SkillEffectManager self, string effectName, Unit unit)
        {
            if (!self.effectDict.TryAdd(effectName, unit.Id))
            {
                Log.Error($"already exist effect, name: {effectName} unit.Id: {unit.Id}");
            }
        }
    }
}