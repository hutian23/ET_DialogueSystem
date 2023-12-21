namespace ET.Client
{
    public class Mai_LightPunch2: BehaviorHandler
    {
        public override int Check(Unit player, BehaviorConfig config)
        {
            //派生
            if (player.ReleasingSkill(MaiSkillType.LightPunch1))
            {
                if (player.CheckSkillCanExit() && Input.Instance.CheckInput(OperaType.RightMoveWasPressed))
                {
                    return 0;
                }
            }
            
            if (player.ReleasingSkill(MaiSkillType.LightPunch2))
            {
                return 0;
            }
            return 1;
        }

        public override async ETTask Handler(Unit player, BehaviorConfig config, ETCancellationToken token)
        {
            SubBehavior lp = config.GetSubBehaviorByName("LightPunch2");
            player.Release_Skill(MaiSkillType.LightPunch2);
            
            NextSkillCor(player,config,token).Coroutine();
            await player.AnimPlayCor(lp, token);
            
            if (token.IsCancel()) return;
            player.DisposeSkill();
        }

        private async ETTask NextSkillCor(Unit player, BehaviorConfig config, ETCancellationToken token)
        {
            player.SetSkillCanExit(false);
            await player.WaitAsync(config.GetInt("CanExitFrame"), token);
            if (token.IsCancel()) return;
            player.SetSkillCanExit(true);
        }
    }
}