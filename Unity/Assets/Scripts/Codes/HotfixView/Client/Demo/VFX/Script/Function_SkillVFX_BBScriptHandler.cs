using System.Text.RegularExpressions;
using UnityEngine;

namespace ET.Client
{
    public class Function_SkillVFX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SkillVFX";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SkillVFX: (?<EffectName>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            Unit caster = parser.GetParent<Unit>();
            Unit vfx = BattleSceneManager.Instance.AddChild<Unit, int>(1001);

            // 设置 vfx 父子关系
            vfx.AddComponent<GameObjectComponent>().GameObject = GameObjectPoolHelper.GetObjectFromPool(match.Groups["EffectName"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}