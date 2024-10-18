using System.Collections.Generic;
using Timeline;

namespace ET.Client
{
    [ChildOf(typeof (SkillBuffer))]
    public class SkillInfo: Entity, IAwake, IDestroy
    {
        public BBTimeline Timeline;

        public string behaviorName;
        public int behaviorOrder;
        public MoveType moveType;
        
        public int order;

        //Trigger
        public List<string> opLines = new();
    }

    public enum MoveType
    {
        None = 0,
        Transition = 1,
        Move = 2,
        Normal = 3,
        Special = 4,
        Super = 5
    }
}