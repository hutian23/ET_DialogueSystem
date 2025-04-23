using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_SoftZone_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_SoftZone";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_SoftZone: (?<CenterX>.*?), (?<CenterY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["CenterX"].Value, out long centerX) ||
                !long.TryParse(match.Groups["CenterY"].Value, out long centerY))
            {
                Log.Error($"cannot format to long!");
                return Status.Failed;
            }
            //1. 初始化
            BBParser _parser = VirtualCameraManager.Instance.GetParent<Unit>().GetComponent<BBParser>();
            _parser.TryRemoveParam("VC_SoftZone_X");
            _parser.TryRemoveParam("VC_SoftZone_Y");
            //2. 注册变量
            _parser.RegistParam("VC_SoftZone_X", centerX / 100f);
            _parser.RegistParam("VC_SoftZone_Y", centerY / 100f);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}