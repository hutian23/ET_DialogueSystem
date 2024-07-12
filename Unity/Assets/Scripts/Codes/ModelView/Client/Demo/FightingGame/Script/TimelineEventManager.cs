using System.Collections.Generic;
using Timeline;

namespace ET.Client
{
    [ComponentOf(typeof (TimelineComponent))]
    public class TimelineEventManager: Entity, IAwake, IDestroy
    {
        //trackName --- track parser
        public Dictionary<string, long> parserDict = new();

        //收集当前帧的帧事件
        public Queue<EventInfo> workQueue = new();
    }
}