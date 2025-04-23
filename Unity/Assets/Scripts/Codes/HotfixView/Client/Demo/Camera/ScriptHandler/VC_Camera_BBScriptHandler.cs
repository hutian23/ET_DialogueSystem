using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    public class VC_Camera_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Camera";
        }

        //VC_Camera: DefaultCamera;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"VC_Camera: (?<Name>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            BBParser _parser = VirtualCameraManager.Instance.GetParent<Unit>().GetComponent<BBParser>();
            _parser.Cancel();
            _parser.Invoke(_parser.GetFunctionPointer(match.Groups["Name"].Value, "Main"), _parser.CancellationToken).Coroutine();

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}