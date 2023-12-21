namespace ET.Client
{
    public class Mai_Slash2ToPunch: BehaviorHandler
    {
        public override int Check(Unit player, BehaviorConfig config)
        {
            if (player.ReleasingSkill(MaiSkillType.Slash2ToPunch))
            {
                return 0;
            }
            if (player.ReleasingSkill(MaiSkillType.HeavySlash2))
            {
                if (player.CheckSkillCanExit() && Input.Instance.CheckInput(OperaType.RightMoveWasPressed)) return 0;
            }
            return 1;
        }

        public override async ETTask Handler(Unit player, BehaviorConfig config, ETCancellationToken token)
        {
            SubBehavior hs2Top = config.GetSubBehaviorByName("Slash2ToPunch");
            player.Release_Skill(MaiSkillType.Slash2ToPunch);

            await player.AnimPlayCor(hs2Top, token);

            if (token.IsCancel()) return;
            player.DisposeSkill();
        }
        
        
    }
}