using System.Text.RegularExpressions;

namespace ET.Client
{
    public class RegistEnhanceInput_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistEnhanceInput";
        }

        // RegistEnhanceInput: 5MPPressed, BuffFrame;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"RegistEnhanceInput: (?<InputType>\w+), (?<BuffFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["BuffFrame"].Value, out int buffFrame))
            {
                Log.Error($"matched failed");
                return Status.Failed;
            }
            
            EnhanceInputComponent EInput = parser.GetComponent<EnhanceInputComponent>();
            EInput.RegistEnhanceInput(match.Groups["InputType"].Value, buffFrame);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}