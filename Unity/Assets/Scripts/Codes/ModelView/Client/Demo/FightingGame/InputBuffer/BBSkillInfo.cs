namespace ET.Client
{
    [ChildOf(typeof (BBInputComponent))]
    public class BBSkillInfo: Entity, IAwake<uint>, IDestroy
    {
        public uint targetID;
        public uint skillType;
        public uint order;
        public string tag;
    }
}