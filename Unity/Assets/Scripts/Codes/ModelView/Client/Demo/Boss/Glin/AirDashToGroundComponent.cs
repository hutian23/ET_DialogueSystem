namespace ET.Client
{
    [ComponentOf(typeof(BBParser))]
    public class AirDashToGroundComponent : Entity, IAwake, IDestroy, IPostStep, IGizmosUpdate
    {
        public bool OnGround;
    }
} 