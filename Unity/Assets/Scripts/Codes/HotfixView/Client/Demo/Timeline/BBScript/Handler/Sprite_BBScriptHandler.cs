using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Sprite_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Sprite";
        }

        //Sprite: 'rg000_1',3;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Sprite:\s*'([^']+)',(\d+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            string spriteName = match.Groups[1].Value;
            if (!int.TryParse(match.Groups[2].Value, out int param))
            {
                Log.Warning($"cannot parse {match.Groups[2].Value} to int!");
                return Status.Failed;
            }

            BBAnimComponent animComponent = parser.GetParent<DialogueComponent>().GetComponent<BBAnimComponent>();
            BBKeyframe keyframe = animComponent.GetKeyframe(spriteName);
            animComponent.SetSprite(keyframe.sprite);

            await parser.GetParent<DialogueComponent>().GetComponent<BBTimerComponent>().WaitAsync(param, token);
            return token.IsCancel()? Status.Failed : Status.Success;
        }
    }
}