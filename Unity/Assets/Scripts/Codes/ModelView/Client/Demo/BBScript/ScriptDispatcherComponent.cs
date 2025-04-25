using System;
using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class ScriptDispatcherComponent: Entity, IAwake, IDestroy, ILoad
    {
        [StaticField]
        public static ScriptDispatcherComponent Instance;
        public Dictionary<string, BBScriptHandler> BBScriptHandlers = new();
        public Dictionary<string, BBTriggerHandler> BBTriggerHandlers = new();
        public Dictionary<string, InputHandler> InputHandlers = new();
        public Dictionary<Type, Type> BBValueDict = new();
    }
}