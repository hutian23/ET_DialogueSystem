using Box2DSharp.Dynamics;

namespace ET.Client
{
    [ComponentOf]
    public class GroundCheckComponent: Entity, IAwake, IDestroy
    {
        public Fixture groundBox;
    }
}