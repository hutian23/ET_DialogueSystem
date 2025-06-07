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

            if (parser.GetComponent<SkillVFXManager>() == null)
            {
                parser.AddComponent<SkillVFXManager>(true);
            }
            
            Unit caster = parser.GetParent<Unit>();
            Unit vfx = parser.GetComponent<SkillVFXManager>().AddChild<Unit, int>(1001);
            
            // 设置父子关系
            GameObject go = GameObjectPoolHelper.GetObjectFromPool(match.Groups["EffectName"].Value);
            go.transform.SetParent(caster.GetComponent<GameObjectComponent>().GameObject.transform);
            go.transform.localPosition = Vector2.zero;
            go.transform.localScale = Vector3.one;
            
            vfx.AddComponent<GameObjectComponent>().GameObject = go;
            vfx.AddComponent<BBParser>();

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}