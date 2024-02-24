namespace ET.Client
{
    public class Shenxiuliuken_CheckHandler: BBCheckHandler
    {
        public override string GetSkillType()
        {
            return "Ryu_Shen_xiuliuken";
        }

        //需要优化
        public override async ETTask Handle(Unit unit, ETCancellationToken token)
        {
            BBWait bbWait = FTGHelper.GetBBWait(unit);

            WaitInput wait1 = await bbWait.Wait(OP: BBOperaType.DOWN, waitType: FuzzyInputType.AND);
            if (wait1.Error != WaitTypeError.Success) return;

            WaitInput wait2 = await bbWait.Wait(OP: BBOperaType.DOWNRIGHT, waitType: FuzzyInputType.AND, waitFrame: 10);
            if (wait2.Error != WaitTypeError.Success) return;

            WaitInput wait3 = await bbWait.Wait(OP: BBOperaType.RIGHT, waitType: FuzzyInputType.OR, waitFrame: 10);
            if (wait3.Error != WaitTypeError.Success) return;

            WaitInput wait4 = await bbWait.Wait(OP: BBOperaType.DOWN, waitType: FuzzyInputType.OR, waitFrame: 10);
            if (wait4.Error != WaitTypeError.Success) return;

            WaitInput wait5 = await bbWait.Wait(OP: BBOperaType.DOWNRIGHT, waitType: FuzzyInputType.AND, waitFrame: 10);
            if (wait5.Error != WaitTypeError.Success) return;

            WaitInput wait6 = await bbWait.Wait(OP: BBOperaType.RIGHT, waitType: FuzzyInputType.OR, waitFrame: 10);
            if (wait6.Error != WaitTypeError.Success) return;
            if ((wait6.OP & (BBOperaType.LIGHTKICK | BBOperaType.MIDDLEKICK | BBOperaType.HEAVYKICK)) != 0)
            {
                unit.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgFtg>().RefreshUI("真升龙拳");
                return;
            }

            WaitInput wait7 = await bbWait.Wait(OP: BBOperaType.LIGHTKICK | BBOperaType.MIDDLEKICK | BBOperaType.HEAVYKICK,
                waitType: FuzzyInputType.OR, waitFrame: 8);
            if (wait7.Error != WaitTypeError.Success) return;

            unit.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgFtg>().RefreshUI("真升龙拳!!!");

            await ETTask.CompletedTask;
        }
    }
}