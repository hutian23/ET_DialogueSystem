namespace ET.Client
{
    public class Mai_LightPunch1: BehaviorHandler
    {
        public override int Check(Unit player, BehaviorConfig config)
        {
            if (player.ReleasingSkill(MaiSkillType.LightPunch1))
            {
                return 0;
            }

            if (Input.Instance.CheckInput(OperaType.RightMoveWasPressed))
            {
                return 0;
            }

            return 1;
        }

        public override async ETTask Handler(Unit player, BehaviorConfig config, ETCancellationToken token)
        {
            SubBehavior lp = config.GetSubBehaviorByName("LightPunch1");

            player.ReleaseSkill(MaiSkillType.LightPunch1);
            
            //Derive
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