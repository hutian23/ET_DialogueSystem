namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(InputWait))]
    public class Before_BehaviorReload_UpdateFlip : AEvent<BeforeBehaviorReload>
    {
        protected override async ETTask Run(Scene scene, BeforeBehaviorReload args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            b2Body b2Body = b2GameManager.Instance.GetBody(unit.InstanceId);
            InputWait inputWait = unit.GetComponent<TimelineComponent>().GetComponent<InputWait>();

            //2. 转向
            FlipState preFlag = b2Body.Flip;
            FlipState curFlag = preFlag;

            if ((inputWait.Ops & BBOperaType.LEFT) != 0)
            {
                curFlag = FlipState.Left;
            }
            else if ((inputWait.Ops & BBOperaType.RIGHT) != 0)
            {
                curFlag = FlipState.Right;
            }
            else if ((inputWait.Ops & BBOperaType.DOWNLEFT) != 0)
            {
                curFlag = FlipState.Left;
            }
            else if ((inputWait.Ops & BBOperaType.DOWNRIGHT) != 0)
            {
                curFlag = FlipState.Right;
            }

            if (curFlag != preFlag)
            {
                b2Body.SetFlip(curFlag);
                b2Body.SetUpdateFlag();
            }

            await ETTask.CompletedTask;
        }
    }
}