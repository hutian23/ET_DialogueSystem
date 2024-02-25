namespace ET.Client
{
    public class Guilie_Fireball_CheckHandler: BBCheckHandler
    {
        public override string GetSkillType()
        {
            return "Guilie_FireBall";
        }

        public override async ETTask Handle(Unit unit, ETCancellationToken token)
        {
            BBWait bbWait = FTGHelper.GetBBWait(unit);

            WaitInput wait1 = await bbWait.Wait(OP: BBOperaType.LEFT | BBOperaType.DOWNLEFT, waitType: FuzzyInputType.OR);
            if (wait1.Error != WaitTypeError.Success) return;

            long preFrame = FTGHelper.GetCurFrame(unit); //开始蓄力的第一帧
            WaitInput wait2 = await bbWait.Wait(OP: BBOperaType.LEFT | BBOperaType.DOWNLEFT, waitType: FuzzyInputType.Hold);
            if (wait2.Error != WaitTypeError.Success) return;

            //最少蓄力45帧
            long curFrame = FTGHelper.GetCurFrame(unit);
            if (curFrame - preFrame < 45) return;
            //第46帧退出蓄力，并且这一帧包含 前 指令，跳过这个阶段的犹豫期
            bool needWait = !(curFrame - preFrame >= 46 && (wait2.OP & BBOperaType.RIGHT) != 0);
            if (needWait)
            {
                WaitInput wait3 = await bbWait.Wait(OP: BBOperaType.RIGHT,
                    waitType: FuzzyInputType.AND, waitFrame: 10);
                if (wait3.Error != WaitTypeError.Success) return;
            }

            WaitInput wait4 = await bbWait.Wait(OP: BBOperaType.LIGHTPUNCH | BBOperaType.MIDDLEPUNCH | BBOperaType.HEAVYPUNCH,
                waitType: FuzzyInputType.OR, 10);
            if (wait4.Error != WaitTypeError.Success) return;

            unit.ClientScene().GetComponent<UIComponent>().GetDlgLogic<DlgFtg>().RefreshUI("Shooting");

            await ETTask.CompletedTask;
        }
    }
}