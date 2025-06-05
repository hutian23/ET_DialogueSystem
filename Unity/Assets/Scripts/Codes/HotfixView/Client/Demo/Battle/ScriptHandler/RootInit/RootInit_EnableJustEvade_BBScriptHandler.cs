using System.Text.RegularExpressions;

namespace ET.Client
{
    public class RootInit_EnableJustEvade_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableJustEvade";
        }

        // JustEvade: ChargeFrame;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "EnableJustEvade: (?<ChargeFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["ChargeFrame"].Value, out int chargeFrame))
            {
                Log.Error("matched failed");
                return Status.Failed;
            }

            BuffManager buffManager = parser.GetParent<Unit>().GetComponent<BuffManager>();
            buffManager.RemoveComponent<JustEvadeAbility>();
            buffManager.AddComponent<JustEvadeAbility, int>(chargeFrame);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}