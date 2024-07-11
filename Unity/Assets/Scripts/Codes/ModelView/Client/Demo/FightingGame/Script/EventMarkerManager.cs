using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (TimelineComponent))]
    public class EventMarkerManager: Entity, IAwake, IDestroy
    {
        public Dictionary<string, ScriptParser> Parsers = new();
    }
}