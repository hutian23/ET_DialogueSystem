using System.Text.RegularExpressions;

namespace ET.Client
{
    public class LoadSpriteAtlasDialogueScriptHandler: DialogueScriptHandler
    {
        public override string GetOPType()
        {
            return "LoadSpriteAtlas";
        }

        //LoadSpriteAtlas name = RagnaAnim;
        public override async ETTask Handle(Unit unit, DialogueNode node, string line, ETCancellationToken token)
        {
            Match match = Regex.Match(line, @"LoadSpriteAtlas name = (?<sprite>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(line);
                return;
            }

            await ResourcesComponent.Instance.LoadBundleAsync($"{match.Groups["sprite"].Value}.unity3d");
        }
    }
}