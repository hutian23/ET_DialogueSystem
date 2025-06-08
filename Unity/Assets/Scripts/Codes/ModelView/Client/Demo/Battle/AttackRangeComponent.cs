using System.Numerics;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class AttackRangeComponent: Entity, IAwake, IPostStep, IDestroy
    {
        public long _instanceId;
        public Vector2 center;
        public Vector2 size;
        public bool InRage;
    }
}