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
            b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            
            //1. Dash充能
            AirDashAbility ad = buffManager.GetComponent<AirDashAbility>();
            if (ad != null)
            {
                ad.SetDashCount(ad.GetMaxDashCount());
            }
            GroundDashAbility gd = buffManager.GetComponent<GroundDashAbility>();
            if (gd != null)
            {
                gd.SetDashCount(gd.GetMaxDashCount());
            }
            
            //2. Jump充能
            JumpAbility ja = buffManager.GetComponent<JumpAbility>();
            if (ja != null)
            {
                ja.SetJumpCount(ja.GetMaxJumpCount());
            }
            
            //3. 记录落地速度
            AirCheckAbility ac = buffManager.GetComponent<AirCheckAbility>();
            ac.SetLandVel(b2Body.GetVelocity());
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}