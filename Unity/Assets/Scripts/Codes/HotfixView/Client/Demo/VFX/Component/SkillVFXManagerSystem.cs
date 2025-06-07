namespace ET.Client
{
    [FriendOf(typeof(SkillVFXManager))]
    public static class SkillVFXManagerSystem
    {
        public class SkillEffectAwakeSystem : AwakeSystem<SkillVFXManager>
        {
            protected override void Awake(SkillVFXManager self)
            {
                self.vfxDict.Clear();
            }
        }

        public class SkillEffectDestroySystem : DestroySystem<SkillVFXManager>
        {
            protected override void Destroy(SkillVFXManager self)
            {
                self.vfxDict.Clear();
            }
        }

        public static void RegistSkillEffect(this SkillVFXManager self, string effectName, Unit unit)
        {
            if (!self.vfxDict.TryAdd(effectName, unit.Id))
            {
                Log.Error($"already exist effect, name: {effectName} unit.Id: {unit.Id}");
            }
        }
    }
}