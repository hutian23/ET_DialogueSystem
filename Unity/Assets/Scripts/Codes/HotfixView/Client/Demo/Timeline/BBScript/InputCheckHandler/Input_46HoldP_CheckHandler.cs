namespace ET.Client
{
    public class Input_46HoldP_CheckHandler: BBCheckHandler
    {
        public override string GetBehaviorType()
        {
            return "46HoldP";
        }

        public override async ETTask<Status> Handle(Unit unit, ETCancellationToken token)
        {
            // BBWait bbWait = FTGHelper.GetBBWait(unit);
            //
            // WaitInput wait1 = await bbWait.Wait(OP: BBOperaType.LEFT | BBOperaType.DOWNLEFT | BBOperaType.UPLEFT, waitType: FuzzyInputType.OR);
            // if (wait1.Error != WaitTypeError.Success) return Status.Failed;
            //
            // long preFrame = FTGHelper.GetCurFrame_InputCor(unit); //开始蓄力的第一帧
            // WaitInput wait2 = await bbWait.Wait(OP: BBOperaType.LEFT | BBOperaType.DOWNLEFT, waitType: FuzzyInputType.Hold);
            // if (wait2.Error != WaitTypeError.Success) return Status.Failed;
            //
            // //最少蓄力45帧
            // long curFrame = FTGHelper.GetCurFrame_InputCor(unit);
            // if (curFrame - preFrame < 45) return Status.Failed;
            // //第46帧退出蓄力，并且这一帧包含 前 指令，跳过这个阶段的犹豫期
            // bool needWait = !(curFrame - preFrame >= 46 && (wait2.OP & BBOperaType.RIGHT) != 0);
            // if (needWait)
            // {
            //     WaitInput wait3 = await bbWait.Wait(OP: BBOperaType.RIGHT, waitType: FuzzyInputType.AND, waitFrame: 10);
            //     if (wait3.Error != WaitTypeError.Success) return Status.Failed;
            // }
            //
            // WaitInput wait4 = await bbWait.Wait(OP: BBOperaType.LIGHTPUNCH | BBOperaType.MIDDLEPUNCH | BBOperaType.HEAVYPUNCH, waitType: FuzzyInputType.OR, 10);
            // if (wait4.Error != WaitTypeError.Success) return Status.Failed;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}