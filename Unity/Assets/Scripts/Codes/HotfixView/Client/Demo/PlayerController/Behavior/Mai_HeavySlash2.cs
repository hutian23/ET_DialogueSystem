namespace ET.Client
{
    public class Mai_HeavySlash2 : BehaviorHandler
    {
        public override int Check(Unit player, BehaviorConfig config)
        {
            if (player.ReleasingSkill(MaiSkillType.HeavySlash2))
            {
                return 0;
            }
            
            if (player.ReleasingSkill(MaiSkillType.HeavySlash1)|| player.ReleasingSkill(MaiSkillType.LightPunch1))
            {
                if (player.CheckSkillCanExit() && Input.Instance.CheckInput(OperaType.LeftMoveWasPressed))
                    return 0;
            }
            
            return 1;
        }

        public override async ETTask Handler(Unit player, BehaviorConfig config, ETCancellationToken token)
        {
            SubBehavior hs2 = config.GetSubBehaviorByName("HeavySlash2");
            Log.Warning(hs2.ClipName + "  " + hs2.frame);
            player.Release_Skill(MaiSkillType.HeavySlash2);
            
            NextSkillCor(player,config,token).Coroutine();
            await player.AnimPlayCor(hs2, token);

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