using UnityEngine;

namespace ET.Client
{
    public static class SkillVFXCasterSystem
    {
        public class SkillVFXCasterAwakeSystem : AwakeSystem<SkillVFXCaster, long>
        {
            protected override void Awake(SkillVFXCaster self, long instanceId)
            {
                self._instanceId = instanceId;
            }
        }
        
        public class SkillVFXCasterDestroySystem : DestroySystem<SkillVFXCaster>
        {
            protected override void Destroy(SkillVFXCaster self)
            {
                self._instanceId = 0;
            }
        }
    }
}