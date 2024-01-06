using System;
using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class DialogueDispatcherComponent : Entity,IAwake,IDestroy, ILoad
    {
        [StaticField]
        public static DialogueDispatcherComponent Instance; 
        
        public Dictionary<Type, NodeHandler> dispatchHandlers = new();

        public Dictionary<Type, NodeCheckHandler> checker_dispatchHandlers = new();
    }

    public enum Status
    {
        Success,
        Failed
    }
}