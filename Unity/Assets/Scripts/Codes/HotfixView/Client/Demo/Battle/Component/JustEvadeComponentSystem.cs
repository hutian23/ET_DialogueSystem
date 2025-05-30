using System.Numerics;

namespace ET.Client
{
    public static class JustEvadeComponentSystem
    {
        public class JustEvadeComponentDestroySystem : DestroySystem<JustEvadeComponent>
        {
            protected override void Destroy(JustEvadeComponent self)
            {
                self.token.Cancel();
                self.boxOffset = Vector2.Zero;
                self.boxSize = Vector2.Zero;
                self.startFrame = 0;
                self.lastFrame = 0;
            }
        }
    }
}