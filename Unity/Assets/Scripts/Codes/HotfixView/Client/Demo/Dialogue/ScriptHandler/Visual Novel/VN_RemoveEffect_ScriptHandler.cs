using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VN_RemoveEffect_ScriptHandler : ScriptHandler
    {
        public override string GetOPType()
        {
            return "VN_RemoveEffect";
        }

        // VN_RemoveEffect name = hold_it;
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"VN_RemoveEffect name = (?<name>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }
            
            var name = match.Groups["name"].Value;
            unit.GetComponent<DialogueComponent>().GetComponent<EffectManager>().RemoveEffect(name);
            await ETTask.CompletedTask;
        }
    }
}