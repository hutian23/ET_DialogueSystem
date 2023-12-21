namespace ET.Client
{
    public class Mai_Punch2ToSlash: BehaviorHandler
    {
        public override int Check(Unit player, BehaviorConfig config)
        {
            if (player.ReleasingSkill(MaiSkillType.Punch2ToSlash))
            {
                return 0;
            }

            if (player.ReleasingSkill(MaiSkillType.LightPunch2))
            {
                if (player.CheckSkillCanExit() && Input.Instance.CheckInput(OperaType.LeftMoveWasPressed)) return 0;
            }

            return 1;
        }

        public override async ETTask Handler(Unit player, BehaviorConfig config, ETCancellationToken token)
        {
            SubBehavior ptohs = config.GetSubBehaviorByName("Punch2ToSlash");
            player.Release_Skill(MaiSkillType.Punch2ToSlash);

            await player.AnimPlayCor(ptohs, token);

            if (token.IsCancel()) return;
            player.DisposeSkill();
        }
    }
}