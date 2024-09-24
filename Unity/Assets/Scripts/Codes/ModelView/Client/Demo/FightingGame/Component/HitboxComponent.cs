using System.Collections.Generic;
using Box2DSharp.Dynamics;
using Timeline;

namespace ET.Client
{
    [ComponentOf(typeof (TimelineComponent))]
    public class HitboxComponent: Entity, IAwake, IDestroy
    {
        public HitboxKeyframe keyFrame;
        
        //b2World
        public Body B2Body;

        public List<Fixture> Fixtures = new();
    }
}