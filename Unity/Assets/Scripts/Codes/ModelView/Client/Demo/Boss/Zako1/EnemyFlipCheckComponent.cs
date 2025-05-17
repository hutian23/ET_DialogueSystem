namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class EnemyFlipCheckComponent : Entity, IAwake, IDestroy, IPostStep
    {
        public bool FlipChange;
    }
}