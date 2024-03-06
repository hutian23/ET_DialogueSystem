namespace ET.Client
{
    public class Hadoken_CheckHandler: BBCheckHandler
    {
        public override string GetSkillType()
        {
            return "Ryu_Fireball";
        }

        public override async ETTask Handle(Unit unit, ETCancellationToken token)
        {
            BBWait bbwait = FTGHelper.GetBBWait(unit);

            WaitInput wait1 = await bbwait.Wait(OP: BBOperaType.DOWN | BBOperaType.DOWNLEFT, waitType: FuzzyInputType.OR);
            if (wait1.Error != WaitTypeError.Success) return;

            WaitInput wait2 = await bbwait.Wait(OP: BBOperaType.DOWNRIGHT, waitType: FuzzyInputType.AND, waitFrame: 10);
            if (wait2.Error != WaitTypeError.Success) return;

            WaitInput wait3 = await bbwait.Wait(OP: BBOperaType.RIGHT | BBOperaType.UPRIGHT, waitType: FuzzyInputType.OR, waitFrame: 5);
            if (wait3.Error != WaitTypeError.Success) return;
            //最后的攻击键和方向键一起按下
            if ((wait3.OP & (BBOperaType.LIGHTPUNCH | BBOperaType.MIDDLEPUNCH | BBOperaType.HEAVYPUNCH)) != 0)
            {
                return;
            }
            
            WaitInput wait4 = await bbwait.Wait(OP: BBOperaType.LIGHTPUNCH | BBOperaType.MIDDLEPUNCH | BBOperaType.HEAVYPUNCH, FuzzyInputType.OR, waitFrame: 5);
            if (wait4.Error != WaitTypeError.Success) return;
            
            await ETTask.CompletedTask;
        }
    }
}