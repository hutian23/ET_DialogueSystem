using Box2DSharp.Dynamics;
using ET.Event;

namespace ET.Client
{
    [Invoke]
    public class HandleEndContactCallback : AInvokeHandler<EndContactCallback>
    {
        public override void Handle(EndContactCallback args)
        {
            Fixture fixtureA = args.Contact.FixtureA;
            Fixture fixtureB = args.Contact.FixtureB;

            long instanceIdA = (long)fixtureA.UserData;
            long instanceIdB = (long)fixtureB.UserData;
            
            b2Box boxA = Root.Instance.Get(instanceIdA) as b2Box;
            b2Box boxB = Root.Instance.Get(instanceIdB) as b2Box;
            if (boxA == null || boxB == null)
            {
                return;
            }
            
            // TriggerExit事件
            if (boxA.GetTrigger() || boxB.GetTrigger())
            {
                if (boxA.GetTriggerExitId() != 0)
                {
                    EventSystem.Instance.Invoke(boxA.GetTriggerExitId(), new TriggerExitCallback()
                    {
                        buffer = new CollisionBuffer()
                        {
                            instanceIdA = boxA.InstanceId,
                            instanceIdB = boxB.InstanceId,
                            Contact = args.Contact
                        }
                    });
                }

                if (boxB.GetTriggerExitId() != 0)
                {
                    EventSystem.Instance.Invoke(boxB.GetTriggerExitId(), new TriggerExitCallback()
                    {
                        buffer = new CollisionBuffer()
                        {
                            instanceIdA =  boxB.InstanceId,
                            instanceIdB = boxA.InstanceId,
                            Contact = args.Contact
                        }
                    });
                }
            }
            
            // CollisionExit事件
            else
            {
                if (boxA.GetCollisionExitId() != 0)
                {
                    EventSystem.Instance.Invoke(boxA.GetCollisionExitId(), new CollisionExitCallback()
                    {
                        buffer = new CollisionBuffer()
                        {
                            instanceIdA =  boxA.InstanceId,
                            instanceIdB = boxB.InstanceId,
                            Contact = args.Contact
                        }
                    });
                }

                if (boxB.GetCollisionExitId() != 0)
                {
                    EventSystem.Instance.Invoke(boxA.GetCollisionExitId(), new CollisionExitCallback()
                    {
                        buffer = new CollisionBuffer()
                        {
                            instanceIdA =  boxB.InstanceId,
                            instanceIdB = boxA.InstanceId,
                            Contact = args.Contact
                        }
                    });
                }
            }
        }
    }
}