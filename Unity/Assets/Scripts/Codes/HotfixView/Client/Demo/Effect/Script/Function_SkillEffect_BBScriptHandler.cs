using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_SkillEffect_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SkillEffect";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SkillEffect: (?<EffectName>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            // if (parser.GetComponent<SkillEffectManager>() == null)
            // {
            //     parser.AddComponent<SkillEffectManager>(true);
            // }
            // GameObject go = GameObjectPoolHelper.GetObjectFromPool(match.Groups["EffectName"].Value);
            // effect.AddComponent<GameObjectComponent>().GameObject = go;
            // go.transform.position = new Vector3(0, 0, 0);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}