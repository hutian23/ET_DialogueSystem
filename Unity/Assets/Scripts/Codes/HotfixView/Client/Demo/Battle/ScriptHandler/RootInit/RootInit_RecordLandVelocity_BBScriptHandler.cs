namespace ET.Client
{
    public class RootInit_RecordLandVelocity_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RecordLandVelocity";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BehaviorMachine machine = parser.GetParent<Unit>().GetComponent<BehaviorMachine>();
            B2Unit b2Unit = parser.GetParent<Unit>().GetComponent<B2Unit>();
            
            if (!machine.ContainParam("LandVelocity"))
            {
                machine.RegistParam("LandVelocity", 0f);
            }

            machine.UpdateParam("LandVelocity", b2Unit.GetVelocity().Y);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}