namespace ET.Client
{
    public class Hadoken_CheckHandler : BBCheckHandler
    {
        public override string GetSkillType()
        {
            return "Ryu_Fireball";
        }

        public override async ETTask Handler(Unit unit, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
        }

        // public override int Check(Unit unit, object nodeCheckConfig)
        // {
        //     // long curFrame = FTGHelper.GetCurFrame(unit);
        //     // foreach (var opInfo in FTGHelper.GetBuffer(unit))
        //     // {
        //     //     
        //     // }
        //     return 0;
        // }
    }
}