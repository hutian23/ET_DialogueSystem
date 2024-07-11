using Timeline;
using UnityEngine;

namespace ET.Client
{
    [Invoke]
    public class TimelineEventCallback : AInvokeHandler<EventMarkerCallback>
    {
        public override void Handle(EventMarkerCallback args)
        {
        }
    }
}