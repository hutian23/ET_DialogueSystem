using System.Numerics;
using Box2DSharp.Common;
using Testbed.Abstractions;

namespace ET.Client
{
    [FriendOf(typeof(b2Box))]
    public static class b2BoxSystem
    {
        public class b2BoxDestroySystem : DestroySystem<b2Box>
        {
            // ReSharper disable Unity.PerformanceAnalysis
            protected override void Destroy(b2Box self)
            {
                self.GetParent<b2Body>().DestroyFixture(self.fixture);
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
                b2WorldManager.Instance.DrawShape(self.fixture.Shape, b2Body.GetTransform().Position, b2Body.GetTransform().Rotation.Angle * UnityEngine.Mathf.Rad2Deg, color);
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

        public static LayerType GetLayerType(this b2Box self)
        {
            return self.LayerType;
        }

        public static TagType GetTagType(this b2Box self)
        {
            return self.TagType;
        }

        public static bool GetTrigger(this b2Box self)
        {
            return self.IsTrigger;
        }

        public static int GetTriggerEnterId(this b2Box self)
        {
            return self.TriggerEnterId;
        }

        public static int GetTriggerStayId(this b2Box self)
        {
            return self.TriggerStayId;
        }
        
        public static int GetTriggerExitId(this b2Box self)
        {
            return self.TriggerExitId;
        }


        public static int GetCollisionEnterId(this b2Box self)
        {
            return self.CollisionEnterId;
        }
        
        public static int GetCollisionStayId(this b2Box self)
        {
            return self.CollisionStayId;
        }

        public static int GetCollisionExitId(this b2Box self)
        {
            return self.CollisionExitId;
        }
    }
}