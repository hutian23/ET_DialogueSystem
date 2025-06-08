using System.Collections.Generic;
using System.Numerics;
using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(AttackRangeComponent))]
    public static class AttackRangeComponentSystem
    {
        [FriendOf(typeof(b2Body))]
        public class AttackRangeComponentPostStepSystem : PostStepSystem<AttackRangeComponent>
        {
            protected override void PosStepUpdate(AttackRangeComponent self)
            {
                BBParser parser = self.GetParent<BBParser>();
                Unit unit = parser.GetParent<Unit>();
                b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);
                b2Box b2Box = b2Body.GetBox("AttackRangeBox");

                self.InRage = false;
                
                Queue<CollisionBuffer> bufferQueue = b2Body.triggerStayBuffers;
                int count = bufferQueue.Count;
                while (count -- > 0)
                {
                    CollisionBuffer buffer = bufferQueue.Dequeue();
                    bufferQueue.Enqueue(buffer);
                    
                    b2Box boxA = Root.Instance.Get(buffer.instanceIdA) as b2Box;
                    b2Box boxB = Root.Instance.Get(buffer.instanceIdB) as b2Box;
                    if (boxA.Id != b2Box.Id || boxB.GetBoxType() is not HitboxType.Squash) continue;

                    self.InRage = true;
                    return;
                }
            }
        }

        public class AttackRangeComponentDestroySystem : DestroySystem<AttackRangeComponent>
        {
            protected override void Destroy(AttackRangeComponent self)
            {
                b2Body b2Body = b2WorldManager.Instance.GetBody(self._instanceId);
                b2Body.DestroyBox("AttackRangeBox");
                self._instanceId = 0;
                
                self.size = Vector2.Zero;
                self.center = Vector2.Zero;

                self.InRage = false;
            }
        }

        public static void Init(this AttackRangeComponent self, Vector2 center, Vector2 size)
        {
            self._instanceId = self.GetParent<BBParser>().GetParent<Unit>().InstanceId;
            self.center = center;
            self.size = size;
            self.InRage = false;

            b2BoxDef boxDef = new()
            {
                layerType = LayerType.Unit,
                IsTrigger = true,
                Name = "AttackRangeBox",
                Center = self.center,
                Size = self.size,
                HitboxType = HitboxType.None,
                TriggerEnterId = TriggerEnterType.HandleCallback,
                TriggerStayId = TriggerStayType.HandleCallback,
                TriggerExitId = TriggerExitType.HandleCallback,
                CollisionEnterId = CollisionEnterType.HandleCallback,
                CollisionStayId = CollisionStayType.HandleCallback,
                CollisionExitId = CollisionExitType.HandleCallback
            };
            EventSystem.Instance.Invoke<CreateB2BoxCallback, b2Box>(new CreateB2BoxCallback()
            {
                instanceId = self.GetParent<BBParser>().GetParent<Unit>().InstanceId,
                boxDef = boxDef
            });
        }

        public static bool InAttackRange(this AttackRangeComponent self)
        {
            return self.InRage;
        }
    }
}