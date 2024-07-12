using System.Collections.Generic;
using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof (ScriptDispatcherComponent))]
    public class StartTimeline_ScriptHandler: ScriptHandler
    {
        public override string GetOpType()
        {
            return "StartTimeline";
        }

        public override async ETTask<Status> Handle(ScriptParser parser, ScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetUnit();

            BBTimerComponent timerComponent = unit.GetComponent<TimelineComponent>().GetComponent<BBTimerComponent>();
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            RuntimePlayable runtimePlayable = timelineComponent.GetTimelinePlayer().RuntimeimePlayable;

            int frame = 0;
            while (true)
            {
                if (token.IsCancel())
                {
                    return Status.Failed;
                }

                //编辑器事件
                EventInfo info = runtimePlayable.Timeline.GetMarker(frame);
                if (info != null)
                {
                    Status ret = await MarkerEventHandle(unit, data, token, info);
                    if (ret != Status.Success)
                    {
                        return ret;
                    }
                }

                runtimePlayable.Evaluate(frame);
                frame++;
                await timerComponent.WaitFrameAsync(token);
            }
        }

        private async ETTask<Status> MarkerEventHandle(Unit unit, ScriptData data, ETCancellationToken token, EventInfo eventInfo)
        {
            if (string.IsNullOrEmpty(eventInfo.Script))
            {
                return Status.Success;
            }

            List<string> opLines = new();
            foreach (var opLine in eventInfo.Script.Split("\n"))
            {
                string op = opLine.Trim();
                if (string.IsNullOrEmpty(op) || op.StartsWith('#')) continue;
                opLines.Add(op);
            }

            //Handle marker op
            //TODO 写的比较简陋 marker中暂时还不能用 SetMarker CallSubCoroutine等方法(目前不能记录指针)
            ScriptParser parser = unit.GetComponent<TimelineComponent>().GetComponent<ScriptParser>();
            foreach (var op in opLines)
            {
                parser.ReplaceParam(op, out string opLine);

                //OpType
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    Log.Error($"{opLine}匹配失败!请检查格式");
                    return Status.Failed;
                }

                //Handler
                string opType = match.Value;
                if (!ScriptDispatcherComponent.Instance.ScriptHandlers.TryGetValue(opType, out ScriptHandler handler))
                {
                    Log.Error($"not found script handler:{opType}");
                    return Status.Failed;
                }

                ScriptData scriptData = ScriptData.Create(opLine, data.coroutineID);
                Status ret = await handler.Handle(parser, scriptData, token);
                scriptData.Recycle();
                if (ret != Status.Success || token.IsCancel())
                {
                    return ret;
                }
            }

            return Status.Success;
        }
    }
}