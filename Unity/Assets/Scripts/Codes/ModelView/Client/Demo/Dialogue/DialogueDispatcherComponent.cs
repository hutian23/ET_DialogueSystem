using System;
using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class DialogueDispatcherComponent : Entity,IAwake,IDestroy, ILoad
    {
        [StaticField]
        public static DialogueDispatcherComponent Instance; 
        
        public Dictionary<Type, NodeHandler> dispatchHandler = new();
    }

    public enum Status
    {
        Success,
        Failed
    }
}