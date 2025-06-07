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
            Unit effect = BulletManager.Instance.AddChild<Unit, int>(1001);
            
            GameObject go = GameObjectPoolHelper.GetObjectFromPool(match.Groups["EffectName"].Value);
            effect.AddComponent<GameObjectComponent>().GameObject = go;
            effect.AddComponent<SkillVFXCaster, long>(caster.InstanceId, true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}