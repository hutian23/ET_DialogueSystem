using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2Box))]
    public class HandleUpdateHitBoxCallback : AInvokeHandler<UpdateHitboxCallback>
    {
        public override void Handle(UpdateHitboxCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            if (timelineComponent == null || timelineComponent.InstanceId == 0) return;
            
            //1. 销毁旧的夹具
            Unit unit = timelineComponent.GetParent<Unit>();
            if (unit == null || unit.InstanceId == 0) return;
            
            b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            b2Body.DestroyBoxes(Box2DHelper.HitboxMask);
            
            //2. 根据关键帧更新Hitbox
            foreach (BoxInfo info in args.Keyframe.boxInfos)
            {
                b2BoxDef boxDef = new()
                {
                    layerType = info.layerType,
                    TagType = info.tagType,
                    IsTrigger = info.hitboxType is HitboxType.Squash,
                    Name = info.boxName,
                    Center = info.center.ToVector2(),
                    Size = info.size.ToVector2(),
                    HitboxType = info.hitboxType,
                    TriggerEnterId = TriggerEnterType.HandleCallback,
                    TriggerStayId = TriggerStayType.HandleCallback,
                    TriggerExitId = TriggerExitType.HandleCallback,
                    CollisionEnterId = CollisionEnterType.HandleCallback,
                    CollisionStayId = CollisionStayType.HandleCallback,
                    CollisionExitId = CollisionExitType.HandleCallback
                };
                
                EventSystem.Instance.Invoke(new CreateB2BoxCallback(){instanceId = unit.InstanceId, boxDef = boxDef});
            }
        }
    }
}