using System.Text.RegularExpressions;

namespace ET.Client
{
    public class AnimPlay_ScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "AnimPlay";
        }

        public override async ETTask Handle(Unit unit, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"AnimPlay ""(.*?)"";");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }
            var clipName = match.Groups[1].Value;
            
            unit.GetComponent<AnimatorComponent>().Play(clipName);
            await ETTask.CompletedTask;
        }
    }
}