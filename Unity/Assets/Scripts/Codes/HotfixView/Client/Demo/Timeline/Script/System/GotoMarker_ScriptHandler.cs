using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (ScriptParser))]
    public class GotoMarker_ScriptHandler: ScriptHandler
    {
        public override string GetOpType()
        {
            return "GotoMarker";
        }

        //GotoMarker: 'hutian2501';
        public override async ETTask<Status> Handle(ScriptParser parser, ScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "GotoMarker: '(?<marker>.*?)';");
            if (!match.Success)
            {
                ScriptHelper.ScriptMatchError(data.opLine);
                return Status.Failed;
            }

            //1. find coroutine
            Unit unit = parser.GetUnit();
            parser.subCoroutineDatas.TryGetValue(data.coroutineID, out SubCoroutineData coroutineData);
            if (coroutineData == null)
            {
                Log.Error($"not found coroutine:{data.coroutineID}");
                return Status.Failed;
            }
            
            //2. find marker
            if (!parser.markerMap.TryGetValue(match.Groups["marker"].Value, out int pointer))
            {
                Log.Error($"not found {match.Groups["marker"].Value}");
                return Status.Failed;
            }

            coroutineData.pointer = pointer;
            BBTimerComponent timer = unit.GetComponent<TimelineComponent>().GetComponent<BBTimerComponent>();
            await timer.WaitFrameAsync(token);
            
            return token.IsCancel()? Status.Failed : Status.Success;
        }
    }
}