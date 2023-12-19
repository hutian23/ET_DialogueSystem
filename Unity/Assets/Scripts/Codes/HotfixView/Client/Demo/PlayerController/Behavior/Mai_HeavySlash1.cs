namespace ET.Client
{
    public class Mai_HeavySlash1 : BehaviorHandler
    {
        public override int Check(Unit player, BehaviorConfig config)
        {
            if (Input.Instance.CheckInput(OperaType.LeftMoveWasPressed))
            {
                return 0;
            }
            
            if (player.ReleasingSkill(MaiSkillType.HeavySlash1))
            {
                return 0;
            }
            
            return 1;
        }

        public override async ETTask Handler(Unit player, BehaviorConfig config, ETCancellationToken token)
        {
            SubBehavior hs = config.GetSubBehaviorByName("HeavySlash1");

            player.ReleaseSkill(MaiSkillType.HeavySlash1);
            player.SetSkillCanExit(false);
        
            //Derived
            NextSkillCor(player, config, token).Coroutine();
            
            await player.AnimPlayCor(hs, token);

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