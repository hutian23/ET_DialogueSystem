using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_Bias_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Bias";
        }

        //VC_Bias
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_Bias: (?<CenterX>.*?), (?<CenterY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["CenterX"].Value, out int centerX) || !int.TryParse(match.Groups["CenterY"].Value, out int centerY))
            {
                Log.Error($"cannot format {match.Groups["CenterX"].Value} / {match.Groups["CenterY"].Value} to int");
                return Status.Failed;
            }

            BBParser _parser = VirtualCameraManager.Instance.GetParent<Unit>().GetComponent<BBParser>();
            _parser.TryRemoveParam("VC_Bias_X");
            _parser.TryRemoveParam("VC_Bias_Y");
            _parser.RegistParam("VC_Bias_X", centerX / 100f);
            _parser.RegistParam("VC_Bias_Y", centerY / 100f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}