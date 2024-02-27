using System.Text.RegularExpressions;

namespace ET.Client
{
    public class HitStop_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitStop";
        }

        // HitStop frame = 60;
        public override async ETTask<Status> Handle(Unit unit, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"HitStop frame = (?<FrameCount>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return Status.Failed;
            }

            int.TryParse(match.Groups["FrameCount"].Value, out int frameCount);

            DialogueComponent dialogueComponent = unit.GetComponent<DialogueComponent>();
            dialogueComponent.RemoveComponent<HitStop>();
            dialogueComponent.AddComponent<HitStop, int>(frameCount);

            token.Add(() => { dialogueComponent.RemoveComponent<HitStop>(); });
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}