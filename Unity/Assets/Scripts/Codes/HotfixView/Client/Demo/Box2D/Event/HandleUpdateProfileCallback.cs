using UnityEngine;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof (b2Body))]
    public class HandleUpdateProfileCallback: AInvokeHandler<UpdateUnitProfileCallback>
    {
        public override void Handle(UpdateUnitProfileCallback args)
        {
            if (args.instanceId == 0) return;

            b2Game b2Game = Camera.main.GetComponent<b2Game>();
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            if (timelineComponent == null) return;

            Unit unit = timelineComponent.GetParent<Unit>();
            b2Body b2body = b2GameManager.Instance.GetBody(unit.InstanceId);

            b2Game.Profile = new UnitProfile()
            {
                UnitName = unit.GetComponent<GameObjectComponent>().GameObject.name,
                AngularVelocity = b2body.body.AngularVelocity,
                LinearVelocity = b2body.body.LinearVelocity,
                Position = b2body.body.GetPosition()
            };
        }
    }
}