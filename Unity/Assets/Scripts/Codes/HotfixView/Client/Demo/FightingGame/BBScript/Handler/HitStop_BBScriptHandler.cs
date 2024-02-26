using System.Text.RegularExpressions;

namespace ET.Client
{
    public class HitStop_BBScriptHandler: ScriptHandler
    {
        public override string GetOPType()
        {
            return "HitStop";
        }

        // HitStop frame = 60;
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"HitStop frame = (?<FrameCount>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            int.TryParse(match.Groups["FrameCount"].Value, out int frameCount);

            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            dialogueComponent.RemoveComponent<HitStop>();
            dialogueComponent.AddComponent<HitStop, int>(frameCount);

            token.Add(() => { dialogueComponent.RemoveComponent<HitStop>(); });
            await ETTask.CompletedTask;
        }
    }
}