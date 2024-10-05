namespace ET.Client
{
    public class Shenxiuliuken_CheckHandler: BBCheckHandler
    {
        public override string GetBehaviorType()
        {
            return "236236P";
        }

        //需要优化
        public override async ETTask<Status> Handle(Unit unit, ETCancellationToken token)
        {
            // BBWait bbWait = FTGHelper.GetBBWait(unit);
            //
            // WaitInput wait1 = await bbWait.Wait(OP: BBOperaType.DOWN | BBOperaType.DOWNLEFT, waitType: FuzzyInputType.OR);
            // if (wait1.Error != WaitTypeError.Success) return Status.Failed;
            //
            // WaitInput wait2 = await bbWait.Wait(OP: BBOperaType.DOWNRIGHT, waitType: FuzzyInputType.AND, waitFrame: 10);
            // if (wait2.Error != WaitTypeError.Success) return Status.Failed;
            //
            // WaitInput wait3 = await bbWait.Wait(OP: BBOperaType.RIGHT | BBOperaType.UPRIGHT, waitType: FuzzyInputType.OR, waitFrame: 10);
            // if (wait3.Error != WaitTypeError.Success) return Status.Failed;
            //
            // WaitInput wait4 = await bbWait.Wait(OP: BBOperaType.DOWN | BBOperaType.DOWNLEFT, waitType: FuzzyInputType.OR, waitFrame: 10);
            // if (wait4.Error != WaitTypeError.Success) return Status.Failed;
            //
            // WaitInput wait5 = await bbWait.Wait(OP: BBOperaType.DOWNRIGHT, waitType: FuzzyInputType.AND, waitFrame: 10);
            // if (wait5.Error != WaitTypeError.Success) return Status.Failed;
            //
            // WaitInput wait6 = await bbWait.Wait(OP: BBOperaType.RIGHT | BBOperaType.UPRIGHT, waitType: FuzzyInputType.OR, waitFrame: 10);
            // if (wait6.Error != WaitTypeError.Success) return Status.Failed;
            //
            // //攻击键和最后一个方向键同一帧
            // if ((wait6.OP & (BBOperaType.LIGHTPUNCH | BBOperaType.MIDDLEPUNCH | BBOperaType.HEAVYPUNCH)) != 0) return Status.Success;
            //
            // WaitInput wait7 = await bbWait.Wait(OP: BBOperaType.LIGHTPUNCH | BBOperaType.MIDDLEPUNCH | BBOperaType.HEAVYPUNCH,
            //     waitType: FuzzyInputType.OR, waitFrame: 8);
            // if (wait7.Error != WaitTypeError.Success) return Status.Failed;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}