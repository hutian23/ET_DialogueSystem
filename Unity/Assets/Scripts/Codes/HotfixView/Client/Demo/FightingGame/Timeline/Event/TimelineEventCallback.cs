using Timeline;
using UnityEngine;

namespace ET.Client
{
    [Invoke]
    public class TimelineEventCallback : AInvokeHandler<EventCallback>
    {
        public override void Handle(EventCallback args)
        {
            Debug.LogWarning("Hello world");
        }
    }
}