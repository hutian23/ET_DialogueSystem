namespace ET.Client
{
    public class Knight_Slash : BehaviorHandler
    {
        public override int Check(Unit unit, BehaviorConfig config)
        {
            if (unit.ReleasingSkill(KnightSkillType.Slash))
            {
                return 0;
            }
            if (Input.Instance.CheckInput(OperaType.LeftMoveWasPressed))
            {
                return 0;
            }
            return 1;
        }

        public override async ETTask Handler(Unit unit, BehaviorConfig config, ETCancellationToken token)
        {
            unit.Release_Skill(KnightSkillType.Slash);
            
            await unit.AnimPlayCor(config.GetSubBehaviorByName("Slash"), token);
            if (token.IsCancel()) return;
            unit.DisposeSkill();
        }
    }
}