using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class TimelineManager: Entity, IAwake, IUpdate, IDestroy, ILoad
    {
        [StaticField]
        public static TimelineManager Instance;

        public HashSet<long> instanceIds = new();

        public bool Pause;
    }
}