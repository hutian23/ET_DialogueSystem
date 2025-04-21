namespace ET.Client
{
    [ComponentOf(typeof(BuffManager))]
    public class GravityCheckAbility : Entity, IAwake, IDestroy
    {
        public float gravity;
        public float maxGravity;
        public float maxFall;

        public ETCancellationToken token;
    }
}