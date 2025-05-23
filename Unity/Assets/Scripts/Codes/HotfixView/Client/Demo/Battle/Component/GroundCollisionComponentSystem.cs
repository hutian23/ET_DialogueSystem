using System.Collections.Generic;
using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(GroundCollisionComponent))]
    public static class GroundCollisionComponentSystem
    {
        public class GroundCollisionCallbackDestroySystem : DestroySystem<GroundCollisionComponent>
        {
            protected override void Destroy(GroundCollisionComponent self)
            {
                self.GroundCollision = false;
                self.functionIndex = 0;
                self.buffer = default;
            }
        }

        [FriendOf(typeof(b2Body))]
        [FriendOf(typeof(BBParser))]
        public class GroundCollisionCallbackPostStepSystem : PostStepSystem<GroundCollisionComponent>
        {
            protected override void PosStepUpdate(GroundCollisionComponent self)
            {
                Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
                b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);

                Queue<CollisionBuffer> bufferQueue = body.collisionStayBuffers;
                int count = bufferQueue.Count;
                while (count -- > 0)
                {
                    CollisionBuffer buffer = bufferQueue.Dequeue();
                    bufferQueue.Enqueue(buffer);
                    
                    b2Box boxA = Root.Instance.Get(buffer.instanceIdA) as b2Box;
                    b2Box boxB = Root.Instance.Get(buffer.instanceIdB) as b2Box;
                    if (boxA.GetBoxType() is not HitboxType.Squash || boxB.GetLayerType() is not LayerType.Ground) continue;

                    // 当前帧接触地面
                    self.GroundCollision = true;
                    
                    // 触发落地回调
                    b2Body bodyA = boxA.GetParent<b2Body>();
                    Unit unitA = Root.Instance.Get(bodyA.unitId) as Unit;
                    BBParser parser = unitA.GetComponent<BBParser>();
                    if (self.functionIndex != 0)
                    {
                        self.buffer = buffer;
                        parser.Invoke(self.functionIndex, parser.CancellationToken).Coroutine();
                        self.buffer = default;
                    }
                    
                    return;
                }

                self.GroundCollision = false;
            }
        }

        public static bool GetGroundCollision(this GroundCollisionComponent self)
        {
            return self.GroundCollision;
        }
    }
}