using System.Collections.Generic;

namespace ET.Client
{
    [ChildOf(typeof (BBInputComponent))]
    public class BBSkillInfo: Entity, IAwake, IDestroy
    {
        public uint targetID;
        public int skillType;
        public int order;
        public string tag;
        public string inputChecker;
        public long LastedFrame;
        public List<string> triggers = new();
    }
}