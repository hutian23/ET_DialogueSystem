using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class ScriptDispatcherComponent: Entity, IAwake, IDestroy, ILoad
    {
        [StaticField]
        public static ScriptDispatcherComponent Instance;

        public Dictionary<string, ScriptHandler> ScriptHandlers = new();
        public Dictionary<string, TriggerHandler> TriggerHandlers = new();
    }
}