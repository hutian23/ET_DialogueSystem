using System;
using System.Collections.Generic;
using ET.Client.V_Model;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class DialogueDispatcherComponent: Entity, IAwake, IDestroy, ILoad
    {
        [StaticField]
        public static DialogueDispatcherComponent Instance;

        public Dictionary<Type, NodeHandler> dispatchHandlers = new();

        public Dictionary<Type, NodeCheckHandler> checker_dispatchHandlers = new();

        public Dictionary<string, ScriptHandler> scriptHandlers = new();

        public Dictionary<string, ModelHandler> modelHandlers = new();
    }
}