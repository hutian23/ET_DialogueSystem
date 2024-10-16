﻿namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof (InputWait))]
    [FriendOf(typeof (b2Body))]
    public class AfterBehaviorReload_UpdatePlayerBehavior: AEvent<AfterBehaviorReload>
    {
        protected override async ETTask Run(Scene scene, AfterBehaviorReload args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            SkillBuffer skillBuffer = unit.GetComponent<TimelineComponent>().GetComponent<SkillBuffer>();
            b2Body b2Body = b2GameManager.Instance.GetBody(unit.InstanceId);
            InputWait inputWait = unit.GetComponent<TimelineComponent>().GetComponent<InputWait>();

            //1. 标记当前行为
            skillBuffer.SetCurrentOrder(args.behaviorOrder);
            skillBuffer.ClearTransition();

            //2. 转向
            var preFlag = b2Body.Flip;
            var curFlag = preFlag;

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