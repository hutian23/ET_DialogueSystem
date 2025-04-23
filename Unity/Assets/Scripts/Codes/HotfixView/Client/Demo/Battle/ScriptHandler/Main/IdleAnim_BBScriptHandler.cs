using System.Text.RegularExpressions;

namespace ET.Client
{
    public class IdleAnim_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "IdleAnim";
        }

        //IdleAnim: Rg_IdleAnim;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "IdleAnim: (?<Animation>.*?), (?<WaitFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["WaitFrame"].Value, out int waitFrame))
            {
                Log.Error($"cannot format {match.Groups["WaitFrame"].Value} to int!!");
                return Status.Failed;
            }

            parser.RemoveComponent<IdleAnimComponent>();
            parser.AddComponent<IdleAnimComponent, int, string>(waitFrame, match.Groups["Animation"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}