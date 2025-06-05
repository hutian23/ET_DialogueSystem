using System.Text.RegularExpressions;

namespace ET.Client
{
    public class Function_TCOption_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "TCOption";
        }

        //TargetOption: BehaviorName, BuffFrame;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"TCOption: (?<Option>\w+), (?<BuffFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Success;
            }

            if (!int.TryParse(match.Groups["BuffFrame"].Value, out int buffFrame))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }

            BuffManager buffManager = parser.GetParent<Unit>().GetComponent<BuffManager>();
            OffsetAbility ability = buffManager.GetComponent<OffsetAbility>();
            ability.BuffOption(match.Groups["Option"].Value, buffFrame);
            
            TargetCancelComponent tc = parser.GetComponent<TargetCancelComponent>();
            tc.Add(match.Groups["Option"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}