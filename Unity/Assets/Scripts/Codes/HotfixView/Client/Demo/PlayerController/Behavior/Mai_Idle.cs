namespace ET.Client
{
    public class Mai_Idle: BehaviorHandler
    {
        public override int Check(Unit unit, BehaviorConfig config)
        {
            return 0;
        }

        public override async ETTask Handler(Unit unit, BehaviorConfig config, ETCancellationToken token)
        {
            SubBehavior idle = config.GetSubBehaviorByName("Idle");
            
            unit.ReleaseSkill(MaiSkillType.Idle);
            unit.AnimPlay(idle.ClipName);
            
            await ETTask.CompletedTask;
        }
    }
}