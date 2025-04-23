using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_Damping_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Damping";
        }

        //VC_Damping: 40000, 10000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "VC_Damping: (?<CenterX>.*?), (?<CenterY>.*?);");
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
            _parser.TryRemoveParam("VC_Damping_X");
            _parser.TryRemoveParam("VC_Damping_Y");
            //2. 注册变量
            _parser.RegistParam("VC_Damping_X", centerX / 10000f);
            _parser.RegistParam("VC_Damping_Y", centerY / 10000f);
           
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}