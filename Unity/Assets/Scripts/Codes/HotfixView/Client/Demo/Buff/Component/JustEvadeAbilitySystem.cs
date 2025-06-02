namespace ET.Client
{
    public static class JustEvadeAbilitySystem
    {
        public class JustEvadeAbilityDestroySystem : DestroySystem<JustEvadeAbility>
        {
            protected override void Destroy(JustEvadeAbility self)
            {
                self.cnt = 0;
                self.chargeFrame = 0;
                self.canJustEvade = false;
            }
        }
    }
}