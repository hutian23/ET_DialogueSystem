using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(TimelineComponent))]
    [FriendOf(typeof(TimelineMarkerEvent))]
    public class MarkerEvent_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "MarkerEvent";
        }

        //MarkerEvent: (Mai_GroundDash_GC);
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"MarkerEvent: \((.*?)\)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            //跳过帧事件的代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndMarkerEvent:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = endIndex;

            TimelineComponent timelineComponent = parser.GetParent<Unit>().GetComponent<TimelineComponent>();
            TimelineMarkerEvent markerEvent = timelineComponent.AddChild<TimelineMarkerEvent>();
            timelineComponent.markerEventDict.Add(match.Groups[1].Value, markerEvent.Id);

            //记录帧时间代码块的起始指针和结束指针
            markerEvent.startIndex = startIndex;
            markerEvent.endIndex = endIndex;
            markerEvent.markerName = match.Groups[1].Value;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}