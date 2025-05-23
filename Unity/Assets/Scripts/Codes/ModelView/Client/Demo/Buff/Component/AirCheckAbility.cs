using System.Numerics;

namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class AirCheckAbility : Entity, IAwake, IDestroy, IPostStep
    {
        public bool inAir;
        // 记录落地时刻unit的速度
        public Vector2 landVel;
    }
}