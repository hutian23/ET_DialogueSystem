namespace ET.Client
{
    public class Knight_Idle : BehaviorHandler
    {
        public override int Check(Unit unit, BehaviorConfig config)
        {
            return 0;
        }

        public override async ETTask Handler(Unit unit, BehaviorConfig config, ETCancellationToken token)
        {
            unit.Release_Skill(KnightSkillType.Idle);
            unit.AnimPlay(config.GetSubBehaviorByName("Idle").ClipName);
            await unit.FallCor(token);
        }
    }
}