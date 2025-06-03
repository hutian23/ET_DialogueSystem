using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    public class Function_RegistMarkerEvent_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistMarkerEvent";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"RegistMarkerEvent: (?<MarkerName>\w+), (?<GroupName>\w+), (?<FunctionName>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //1. 用时添加
            if (parser.GetComponent<MarkerEventManager>() == null)
            {
                parser.AddComponent<MarkerEventManager>();
            }
            
            //2. 帧事件指针
            string markerName = match.Groups["MarkerName"].Value;
            int functionIndex = parser.GetFunctionPointer(match.Groups["GroupName"].Value, match.Groups["FunctionName"].Value);
            
            //3. 添加帧事件
            MarkerEventManager markerEventManager = parser.GetComponent<MarkerEventManager>();
            markerEventManager.RegistMarkerEvent(markerName, functionIndex);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}