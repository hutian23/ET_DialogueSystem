namespace ET.Client
{
    public class Function_EnemyUpdateFlip_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnemyUpdateFlip";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit player = BBUnitHelper.GetPlayer();
            Unit unit = parser.GetParent<Unit>();
            b2Body bodyA = b2WorldManager.Instance.GetBody(player.InstanceId);
            b2Body bodyB = b2WorldManager.Instance.GetBody(unit.InstanceId);

            bodyB.SetFlip(bodyA.GetPosition().X >= bodyB.GetPosition().X? FlipState.Right : FlipState.Left);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}