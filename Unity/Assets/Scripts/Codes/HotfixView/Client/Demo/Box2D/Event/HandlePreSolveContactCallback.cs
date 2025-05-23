using Box2DSharp.Dynamics;
using ET.Event;

namespace ET.Client
{
    [Invoke]
    public class HandlePreSolveContactCallback : AInvokeHandler<PreSolveCallback>
    {
        public override void Handle(PreSolveCallback args)
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

            //触发器不参与碰撞
            if (boxA.GetTrigger() || boxB.GetTrigger())
            {
                args.Contact.SetEnabled(false);
            }
            
            // TriggerStay事件
            if (boxA.GetTrigger() || boxB.GetTrigger())
            {
                if (boxA.GetTriggerStayId() != 0)
                {
                    EventSystem.Instance.Invoke(boxA.GetTriggerStayId(), new TriggerStayCallback()
                    {
                        buffer = new CollisionBuffer()
                        {
                            instanceIdA = boxA.InstanceId,
                            instanceIdB = boxB.InstanceId,
                            Contact = args.Contact
                        }
                    });
                }

                if (boxB.GetTriggerStayId() != 0)
                {
                    EventSystem.Instance.Invoke(boxB.GetTriggerStayId(), new TriggerStayCallback()
                    {
                        buffer = new CollisionBuffer()
                        {
                            instanceIdA = boxB.InstanceId,
                            instanceIdB = boxA.InstanceId,
                            Contact = args.Contact
                        }
                    });
                }
            }
            //CollisionStay事件
            else
            {
                if (boxA.GetCollisionStayId() != 0)
                {
                    EventSystem.Instance.Invoke(boxA.GetCollisionStayId(), new CollisionStayCallback()
                    {
                        buffer = new CollisionBuffer()
                        {
                            instanceIdA = boxA.InstanceId,
                            instanceIdB = boxB.InstanceId,
                            Contact = args.Contact
                        }
                    });
                }

                if (boxB.GetCollisionStayId() != 0)
                {
                    EventSystem.Instance.Invoke(boxB.GetCollisionStayId(), new CollisionStayCallback()
                    {
                        buffer = new CollisionBuffer()
                        {
                            instanceIdA = boxB.InstanceId,
                            instanceIdB = boxA.InstanceId,
                            Contact = args.Contact
                        }
                    });
                }
            }
        }
    }
}