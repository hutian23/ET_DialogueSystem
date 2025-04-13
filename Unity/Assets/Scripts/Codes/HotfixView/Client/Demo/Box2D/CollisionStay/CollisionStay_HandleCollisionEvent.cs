using ET.Event;

namespace ET.Client
{
    [Invoke(CollisionStayType.CollisionEvent)]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(B2Unit))]
    public class CollisionStay_HandleCollisionEvent : AInvokeHandler<CollisionStayCallback>
    {
        public override void Handle(CollisionStayCallback args)
        {
            CollisionInfo info = args.info;

            b2Body b2Body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;

            //Collision缓冲区缓冲碰撞信息
            B2Unit b2Unit = unit.GetComponent<B2Unit>();
            b2Unit.CollisionBuffer.Enqueue(info);
        }
    }
}