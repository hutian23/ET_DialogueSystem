namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class EffectManager : Entity, IAwake, IDestroy
    {
        [StaticField]
        public static EffectManager Instance;
    }
}