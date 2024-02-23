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

            WaitInput wait1 = await bbwait.Wait(OP: TODOperaType.DOWN | TODOperaType.DOWNLEFT, waitType: FuzzyInputType.OR);
            if (wait1.Error != WaitTypeError.Success) return;

            WaitInput wait2 = await bbwait.Wait(OP: TODOperaType.DOWNRIGHT, waitType: FuzzyInputType.AND, waitFrame: 99);
            if (wait2.Error != WaitTypeError.Success) return;

            WaitInput wait3 = await bbwait.Wait(OP: TODOperaType.RIGHT | TODOperaType.UPRIGHT, waitType: FuzzyInputType.OR, waitFrame: 99);
            if (wait3.Error != WaitTypeError.Success) return;

            WaitInput wait4 = await bbwait.Wait(OP: TODOperaType.LIGHTPUNCH | TODOperaType.MIDDLEPUNCH | TODOperaType.HEAVYPUNCH, FuzzyInputType.OR, waitFrame: 99);
            if (wait4.Error != WaitTypeError.Success) return;

            Log.Warning("波动拳!!!");

            await ETTask.CompletedTask;
        }
    }
}