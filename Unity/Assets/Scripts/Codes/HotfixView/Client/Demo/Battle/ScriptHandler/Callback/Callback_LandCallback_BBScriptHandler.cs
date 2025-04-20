namespace ET.Client
{
    public class Callback_LandCallback_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "LandCallback";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            BuffManager buffManager = unit.GetComponent<BuffManager>();
            
            //1. Dash充能
            AirDashAbility ad = buffManager.GetComponent<AirDashAbility>();
            GroundDashAbility gd = buffManager.GetComponent<GroundDashAbility>();
            ad.SetDashCount(ad.GetMaxDashCount());
            gd.SetDashCount(gd.GetMaxDashCount());
            
            //2. Jump充能
            JumpAbility ja = buffManager.GetComponent<JumpAbility>();
            ja.SetJumpCount(ja.GetMaxJumpCount());
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}