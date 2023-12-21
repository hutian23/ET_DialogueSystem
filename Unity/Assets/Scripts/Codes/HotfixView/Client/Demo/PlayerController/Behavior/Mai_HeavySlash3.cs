namespace ET.Client
{
    public class Mai_HeavySlash3 : BehaviorHandler
    {
        public override int Check(Unit player, BehaviorConfig config)
        {
            if (player.ReleasingSkill(MaiSkillType.HeavySlash3))
            {
                return 0;
            }
            if (player.ReleasingSkill(MaiSkillType.HeavySlash2))
            {
                if (player.CheckSkillCanExit() && Input.Instance.CheckInput(OperaType.LeftMoveWasPressed))
                    return 0;
            }
            
            return 1;
        }

        public override async ETTask Handler(Unit player, BehaviorConfig config, ETCancellationToken token)
        {
            SubBehavior hs2 = config.GetSubBehaviorByName("HeavySlash3");
            Log.Warning(hs2.ClipName + "  " + hs2.frame);
            player.Release_Skill(MaiSkillType.HeavySlash3);
            
            NextSkillCor(player,config,token).Coroutine();
            await player.AnimPlayCor(hs2, token);

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