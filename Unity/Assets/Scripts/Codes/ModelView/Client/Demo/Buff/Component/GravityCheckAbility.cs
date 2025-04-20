namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class GravityCheckAbility : Entity, IAwake, IDestroy
    {
        public long timer;
        
        public float gravity;
        public float maxGravity;
        public float maxFall;
    }
}