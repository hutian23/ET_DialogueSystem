using System.Collections.Generic;
using Timeline;

namespace ET.Client
{
    [ChildOf(typeof (SkillBuffer))]
    public class SkillInfo: Entity, IAwake, IDestroy
    {
        public BBTimeline Timeline;

        public int order;

        public List<string> opLines = new();
    }
}