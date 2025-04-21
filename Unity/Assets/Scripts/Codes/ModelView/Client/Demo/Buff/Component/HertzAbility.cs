namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class HertzAbility : Entity, IAwake, IDestroy
    {
        public int Hertz;
    }
}