using Box2DSharp.Dynamics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke]
    public class HandlePreSolveContactCallback : AInvokeHandler<PreSolveCallback>
    {
        public override void Handle(PreSolveCallback args)
        {
            Fixture fixtureA = args.Contact.FixtureA;
            Fixture fixtureB = args.Contact.FixtureB;

            if (fixtureA == null || fixtureB == null || 
                fixtureA.UserData is not FixtureData dataA ||
                fixtureB.UserData is not FixtureData dataB)
            {
                return;
            }
            
            //Trigger Stay
            if (dataA.IsTrigger || dataB.IsTrigger)
            {
                if (dataA.TriggerStayId != 0)
                {
                    EventSystem.Instance.Invoke(dataA.TriggerStayId, new TriggerStayCallback() { info = new CollisionInfo()
                    {
                        fixtureA = fixtureA, fixtureB = fixtureB, dataA = dataA, dataB = dataB, Contact = args.Contact
                    }});
                }

                if (dataB.TriggerStayId != 0)
                {
                    EventSystem.Instance.Invoke(dataB.TriggerStayId, new TriggerStayCallback() { info = new CollisionInfo()
                    {
                        fixtureA = fixtureB, fixtureB = fixtureA, dataA = dataB, dataB = dataA, Contact = args.Contact
                    }});
                }
            }
            else
            {
                if (dataA.CollisionStayId != 0)
                {
                    EventSystem.Instance.Invoke(dataA.CollisionStayId, new CollisionStayCallback() { info = new CollisionInfo() 
                    {
                        fixtureA = fixtureA, fixtureB = fixtureB, dataA = dataA, dataB = dataB, Contact = args.Contact
                    }});
                }

                if (dataB.CollisionStayId != 0)
                {
                    EventSystem.Instance.Invoke(dataB.CollisionStayId, new CollisionStayCallback(){ info = new CollisionInfo()
                    {
                        fixtureA = fixtureB, fixtureB = fixtureA, dataA = dataB, dataB = dataA, Contact = args.Contact
                    }});
                }
            }
        }
    }
}