namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class SkillVFXCaster : Entity, IAwake<long>, IDestroy
    {
        public long _instanceId; // caster.instanceId
    }
}