namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class JumpAbility : Entity, IAwake, IDestroy
    {
        public int JumpCount;
        public int JumpMaxCount;
    }
}