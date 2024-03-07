using System.Text.RegularExpressions;

namespace ET.Client
{
    public class HitStop_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitStop";
        }

        // HitStop: 60;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "HitStop: (?<Frame>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            int.TryParse(match.Groups["Frame"].Value, out int frameCount);

            DialogueComponent dialogueComponent = parser.GetParent<DialogueComponent>();
            dialogueComponent.RemoveComponent<HitStop>();
            dialogueComponent.AddComponent<HitStop, int>(frameCount);

            token.Add(() => { dialogueComponent.RemoveComponent<HitStop>(); });
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}