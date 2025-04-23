using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_DeadZone_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_DeadZone";
        }

        //VC_DeadZone: 30, 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_DeadZone: (?<CenterX>.*?), (?<CenterY>.*?);");
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

            BBParser _parser = VirtualCameraManager.Instance.GetParent<Unit>().GetComponent<BBParser>();
            //1. 初始化
            _parser.TryRemoveParam("VC_DeadZone_X");
            _parser.TryRemoveParam("VC_DeadZone_Y");
            //2. 注册变量
            _parser.RegistParam("VC_DeadZone_X", centerX / 100f);
            _parser.RegistParam("VC_DeadZone_Y", centerY / 100f);
           
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}