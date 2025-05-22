using Box2DSharp.Dynamics;
using ET.Event;

namespace ET.Client
{
    [Invoke]
    public class HandleBeginContactCallback : AInvokeHandler<BeginContactCallback>
    {
        public override void Handle(BeginContactCallback args)
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

            // TriggerEnter事件
            if (boxA.GetTrigger() || boxB.GetTrigger())
            {
                if (boxA.GetTriggerEnterId() != 0)
                {
                    EventSystem.Instance.Invoke(boxA.GetTriggerEnterId(), new TriggerEnterCallback()
                    {
                        buffer = new CollisionBuffer()
                        {
                            BoxA_InstanceId = boxA.InstanceId,
                            BoxB_InstanceId = boxB.InstanceId,
                            Contact = args.Contact
                        }
                    });
                }

                if (boxB.GetTriggerEnterId() != 0)
                {
                    EventSystem.Instance.Invoke(boxB.GetTriggerEnterId(), new TriggerEnterCallback()
                    {
                        buffer = new CollisionBuffer()
                        {
                            BoxA_InstanceId = boxB.InstanceId,
                            BoxB_InstanceId = boxA.InstanceId,
                            Contact = args.Contact
                        }
                    });
                }
            }
            // CollisionEnter事件
            else
            {
                if (boxA.GetCollisionEnterId() != 0)
                {
                    EventSystem.Instance.Invoke(boxA.GetCollisionEnterId(), new CollisionEnterCallback()
                    {
                        buffer = new CollisionBuffer()
                        {
                            BoxA_InstanceId = boxA.InstanceId,
                            BoxB_InstanceId = boxB.InstanceId,
                            Contact = args.Contact
                        }
                    });     
                }

                if (boxB.GetCollisionEnterId() != 0)
                {
                    EventSystem.Instance.Invoke(boxB.GetCollisionEnterId(), new CollisionEnterCallback()
                    {
                        buffer = new CollisionBuffer()
                        {
                            BoxA_InstanceId = boxB.InstanceId,
                            BoxB_InstanceId = boxA.InstanceId,
                            Contact = args.Contact
                        }
                    });
                }
            }
        }
    }
}