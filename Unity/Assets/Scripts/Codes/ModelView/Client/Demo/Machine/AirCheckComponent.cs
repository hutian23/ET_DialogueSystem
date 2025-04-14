namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class AirCheckComponent : Entity, IAwake, IDestroy
    {
        public long timer;
        public bool inAir;
    }
}