using System.Numerics;
using Box2DSharp.Common;
using Testbed.Abstractions;

namespace ET.Client
{
    [FriendOf(typeof(b2Box))]
    public static class b2BoxSystem
    {
        [FriendOf(typeof(b2Body))]
        public class b2BoxDestroySystem : DestroySystem<b2Box>
        {
            protected override void Destroy(b2Box self)
            {
                b2Body b2Body = self.GetParent<b2Body>();
                b2Body.body.DestroyFixture(self.fixture);
                self.fixture = null;
                self.fixtureDef = default;
                self.LayerType = LayerType.None;
                self.TagType = TagType.None;
                self.IsTrigger = false;
                self.Name = string.Empty;
                self.HitboxType = HitboxType.None;
                self.Center = Vector2.Zero;
                self.Size = Vector2.Zero;
                self.TriggerEnterId = 0;
                self.TriggerStayId = 0;
                self.TriggerExitId = 0;
                self.CollisionEnterId = 0;
                self.CollisionStayId = 0;
                self.CollisionExitId = 0;
            }
        }
        
        public class b2BoxGizmosUpdateSystem : GizmosUpdateSystem<b2Box>
        {
            protected override void GizmosUpdate(b2Box self)
            {
                // 是否渲染?
                if ((self.HitboxType is HitboxType.Hit && !Global.Settings.ShowHitBox) || 
                    (self.HitboxType is HitboxType.Hurt && !Global.Settings.ShowHurtBox) ||
                    (self.HitboxType is HitboxType.Throw && !Global.Settings.ShowThrowBox) ||
                    (self.HitboxType is HitboxType.Squash && !Global.Settings.ShowSquashBox) ||
                    (self.HitboxType is HitboxType.Proximity && !Global.Settings.ShowProximityBox) ||
                    (self.HitboxType is HitboxType.Gizmos && !Global.Settings.ShowGizmos))
                {
                    return;
                }

                Color color = self.HitboxType switch
                {
                    HitboxType.Hit => Color.Red,
                    HitboxType.Hurt => Color.Green,
                    HitboxType.Squash => Color.Yellow,
                    HitboxType.Throw => Color.Blue,
                    HitboxType.Proximity => Color.Magenta,
                    HitboxType.Other => Color.Gray,
                    _ => Color.White
                };

                b2Body b2Body = self.GetParent<b2Body>();
                Transform transform = b2Body.GetTransform();
                b2WorldManager.Instance.DrawShape(self.fixture.Shape, transform.Position, transform.Rotation.Angle * UnityEngine.Mathf.Rad2Deg, color);
            }
        }
        
        public static HitboxType GetBoxType(this b2Box self)
        {
            return self.HitboxType;
        }

        public static string GetBoxName(this b2Box self)
        {
            return self.Name;
        }
    }
}