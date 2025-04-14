namespace ET.Client
{
    [ComponentOf(typeof(Unit))]
    public class AirCheckComponent : Entity, IAwake, IDestroy
    {
        public long timer;
        public bool inAir;
    }
}