namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class EffectManager : Entity, IAwake, IDestroy, ILoad
    {
        [StaticField]
        public static EffectManager Instance;
    }
}