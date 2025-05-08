using Timeline;

namespace ET.Client
{
    [Invoke]
    public class HandleUpdateRotationCallback : AInvokeHandler<UpdateRotationCallback>
    {
        public override void Handle(UpdateRotationCallback args)
        {   
            //1. 查询组件
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            Unit unit = timelineComponent.GetParent<Unit>();
            BBParser bbParser = unit.GetComponent<BBParser>();
            RotationComponent rotationComponent = bbParser.GetComponent<RotationComponent>();
            
            //2. PostStep中的Rotation更新顺序为 b2Body.SysTrans() --> RotationComponent.PostStep()
            if (rotationComponent == null)
            {
                return;
            }
            rotationComponent.SetEulerAngles(args.eulerAngles);
        }
    }
}