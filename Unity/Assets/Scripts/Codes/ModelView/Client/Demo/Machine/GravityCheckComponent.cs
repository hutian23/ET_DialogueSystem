namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class GravityCheckComponent : Entity, IAwake, IDestroy
    {
        public long timer;
        
        public float gravity;
        public float maxGravity;
        public float maxFall;
    }
}