namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class AirCheckAbility : Entity, IAwake, IDestroy
    {
        public long timer;
        public bool inAir;
        // 记录落地时刻unit的速度
        public float landV_X;
        public float landV_Y;
    }
}