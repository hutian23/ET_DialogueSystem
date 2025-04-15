using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    public class Function_Event_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Event";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 匹配参数
            Match match = Regex.Match(data.opLine, @"Event: \((.*?)\)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //2. 跳过代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndEvent:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = endIndex;

            //3. 添加组件 
            if (parser.GetComponent<MarkerEventComponent>() == null)
            {
                parser.AddComponent<MarkerEventComponent>();
            }
            
            //4. 添加帧事件
            MarkerEventComponent markerEventComponent = parser.GetComponent<MarkerEventComponent>();
            markerEventComponent.RegistMarkerEvent(new MarkerEvent()
            {
                markerName = match.Groups[1].Value,
                startIndex = startIndex,
                endIndex = endIndex
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}