namespace ET.Client
{
    public class Mai_LightPunch3 : BehaviorHandler
    {
        public override int Check(Unit player, BehaviorConfig config)
        {
            //派生
            if (player.ReleasingSkill(MaiSkillType.LightPunch2))
            {
                if (player.CheckSkillCanExit() && Input.Instance.CheckInput(OperaType.RightMoveWasPressed))
                {
                    return 0;
                }
            }
            
            if (player.ReleasingSkill(MaiSkillType.LightPunch3))
            {
                return 0;
            }
            return 1;
        }

        public override async ETTask Handler(Unit player, BehaviorConfig config, ETCancellationToken token)
        {
            SubBehavior lp = config.GetSubBehaviorByName("LightPunch3");
            player.Release_Skill(MaiSkillType.LightPunch3);
            
            NextSkillCor(player,config,token).Coroutine();
            await player.AnimPlayCor(lp, token);
            
            if (token.IsCancel()) return;
            player.DisposeSkill();
        }

        private async ETTask NextSkillCor(Unit player, BehaviorConfig config, ETCancellationToken token)
        {
            await player.WaitAsync(config.GetInt("CanExitFrame"), token);
            if (token.IsCancel()) return;
            player.SetSkillCanExit(true);
        }
    }
}