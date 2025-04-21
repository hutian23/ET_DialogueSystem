using System.Numerics;

namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class RootMotionComponent : Entity, IAwake, IDestroy, IPreStep
    {
        public Vector2 MotionVel;
    }
}