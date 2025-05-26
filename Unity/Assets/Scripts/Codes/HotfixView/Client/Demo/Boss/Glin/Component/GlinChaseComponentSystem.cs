using UnityEngine;
using Color = Box2DSharp.Common.Color;

namespace ET.Client
{
    [FriendOf(typeof(GlinChaseComponent))]
    public static class GlinChaseComponentSystem
    {
        public class GlinChaseComponentDestroySystem : DestroySystem<GlinChaseComponent>
        {
            protected override void Destroy(GlinChaseComponent self)
            {
                self.minRotate = 0f;
                self.maxRotate = 0f;
                self.curRotate = 0f;
                self.damping = 0f;
                self.token.Cancel();
            }
        }

        public class GlinChaseComponentGizmosUpdateSystem : GizmosUpdateSystem<GlinChaseComponent>
        {
            protected override void GizmosUpdate(GlinChaseComponent self)
            {
                Unit player = BBUnitHelper.GetPlayer(self.ClientScene());
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                b2Body bodyA = b2WorldManager.Instance.GetBody(player.InstanceId);
                b2Body bodyB = b2WorldManager.Instance.GetBody(unit.InstanceId);
                
                b2WorldManager.Instance.DrawPoint(bodyA.GetPosition(), 10f, Color.White);
                b2WorldManager.Instance.DrawPoint(bodyB.GetPosition(), 10f, Color.White);
                b2WorldManager.Instance.DrawSegment(bodyA.GetPosition(), bodyB.GetPosition(), Color.White);
            }
        }
        
        public static async ETTask RotateCor(this GlinChaseComponent self, float minRotate, float maxRotate, float damping)
        {
            self.minRotate = minRotate;
            self.maxRotate = maxRotate;
            self.curRotate = 0;
            self.damping = damping;
            self.token = new ETCancellationToken();
                    
            Unit player = BBUnitHelper.GetPlayer(self.ClientScene());
            Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
            b2Body bodyA = b2WorldManager.Instance.GetBody(player.InstanceId);
            b2Body bodyB = b2WorldManager.Instance.GetBody(unit.InstanceId);
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();

            while (true)
            {
                Vector2 direction = (bodyB.GetPosition() - bodyA.GetPosition()).ToUnityVector2().normalized;
                float targetRotate = Vector2.Angle(direction, Vector2.right) - 90f;
                
                self.curRotate = Mathf.Lerp(self.curRotate, targetRotate, damping * ScriptHelper.FrameLength);
                self.curRotate = Mathf.Clamp(self.curRotate, self.minRotate, self.maxRotate);
                bodyB.SetAngle(self.curRotate);
                 
                await bbTimer.WaitFrameAsync(self.token);
                if (self.token.IsCancel()) return;
            }
        }
    }
}