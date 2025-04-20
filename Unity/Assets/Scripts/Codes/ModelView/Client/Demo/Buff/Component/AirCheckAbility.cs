namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class AirCheckAbility : Entity, IAwake, IDestroy
    {
        public long timer;
        public bool inAir;
    }
}