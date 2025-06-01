using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_Invincible_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Invincible";
        }

        // Invincible: LastFrame;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Invincible: (?<LastFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["LastFrame"].Value, out int lastFrame))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            BuffManager buffManager = parser.GetParent<Unit>().GetComponent<BuffManager>();
            buffManager.RemoveComponent<InvincibleAbility>();
            buffManager.AddComponent<InvincibleAbility, int>(lastFrame, true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}