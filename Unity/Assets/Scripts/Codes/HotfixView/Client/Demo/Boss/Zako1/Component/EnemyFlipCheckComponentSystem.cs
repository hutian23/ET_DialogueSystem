namespace ET.Client
{
    [FriendOf(typeof(EnemyFlipCheckComponent))]
    public static class EnemyFlipCheckComponentSystem
    {
        public class EnemyFlipCheckComponentDestroySystem : DestroySystem<EnemyFlipCheckComponent>
        {
            protected override void Destroy(EnemyFlipCheckComponent self)
            {
                self.FlipChange = false;
            }
        }

        public class EnemyFlipCheckComponentPostStepSystem : PostStepSystem<EnemyFlipCheckComponent>
        {
            protected override void PosStepUpdate(EnemyFlipCheckComponent self)
            {
                Unit unitA = BBUnitHelper.GetPlayer(self.ClientScene());
                Unit unitB = self.GetParent<BBParser>().GetParent<Unit>();
                b2Body bodyA = b2WorldManager.Instance.GetBody(unitA.InstanceId);
                b2Body bodyB = b2WorldManager.Instance.GetBody(unitB.InstanceId);

                FlipState curFlip = bodyB.GetPosition().X >= bodyA.GetPosition().X ? FlipState.Left : FlipState.Right;
                self.FlipChange = curFlip != (FlipState)bodyB.GetFlip();
            }
        }

        public static bool GetFlipChange(this EnemyFlipCheckComponent self)
        {
            return self.FlipChange;
        }
    }
}