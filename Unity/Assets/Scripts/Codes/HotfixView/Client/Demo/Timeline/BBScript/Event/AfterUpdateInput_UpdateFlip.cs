namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof (b2Body))]
    [FriendOf(typeof (InputWait))]
    public class AfterUpdateInput_UpdateFlip: AEvent<AfterUpdateInput>
    {
        protected override async ETTask Run(Scene scene, AfterUpdateInput args)
        {
            InputWait inputWait = Root.Instance.Get(args.instanceId) as InputWait;
            b2Body b2Body = b2GameManager.Instance.GetBody(inputWait.GetParent<TimelineComponent>().GetParent<Unit>().InstanceId);
            
            //2. 翻转指令,当朝向为右时
            // Left <---> Right
            // DownLeft <---> DownRight
            // UpLeft <---> UpRight
            //这里以Flip.Left为默认朝向
            // if (b2Body.Flip is FlipState.Right)
            // {
            //     long op = args.OP;
            //     op &= ~(BBOperaType.LEFT & BBOperaType.RIGHT
            //         & BBOperaType.DOWNLEFT & BBOperaType.DOWNRIGHT
            //         & BBOperaType.UPLEFT & BBOperaType.UPRIGHT);
            //
            //     //翻转
            //     if ((args.OP & BBOperaType.LEFT) != 0)
            //     {
            //         op |= BBOperaType.RIGHT;
            //     }
            //     else if ((args.OP & BBOperaType.RIGHT) != 0)
            //     {
            //         op |= BBOperaType.LEFT;
            //     }
            //     else if ((args.OP & BBOperaType.DOWNLEFT) != 0)
            //     {
            //         op |= BBOperaType.DOWNRIGHT;
            //     }
            //     else if ((args.OP & BBOperaType.DOWNRIGHT) != 0)
            //     {
            //         op |= BBOperaType.DOWNLEFT;
            //     }
            //     else if ((args.OP & BBOperaType.UPLEFT) != 0)
            //     {
            //         op |= BBOperaType.UPRIGHT;
            //     }
            //     else if ((args.OP & BBOperaType.UPRIGHT) != 0)
            //     {
            //         op |= BBOperaType.UPLEFT;
            //     }
            //
            //     inputWait.Ops = op;
            // }
            // Log.Warning("Update Input");
            await ETTask.CompletedTask;
        }
    }
}