using System.Collections.Generic;
using ET.Event;
using Timeline;

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
                self.info = default;
            }
        }

        [FriendOf(typeof(b2Body))]
        [FriendOf(typeof(BBParser))]
        public class GroundCollisionCallbackPostStepSystem : PostStepSystem<GroundCollisionComponent>
        {
            protected override void PosStepUpdate(GroundCollisionComponent self)
            {
                // 查询组件
                BBParser parser = self.GetParent<BBParser>();
                Unit unit = parser.GetParent<Unit>();
                b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);

                // 逐个检测碰撞信息
                Queue<CollisionInfo> infoQueue = b2Body.collisionEnterBuffer;
                int count = infoQueue.Count;
                while (count-- > 0)
                {
                    CollisionInfo info = infoQueue.Dequeue();
                    infoQueue.Enqueue(info);

                    BoxInfo infoA = info.dataA.UserData as BoxInfo;
                    if (infoA.hitboxType is not HitboxType.Squash || info.dataB.LayerMask is not LayerType.Ground) continue;

                    // 当前帧接触地面
                    self.GroundCollision = true;

                    // 地面碰撞回调
                    if (self.functionIndex != 0)
                    {
                        self.info = info;
                        parser.Invoke(self.functionIndex, parser.CancellationToken).Coroutine();
                        self.info = default;
                    }

                    return;
                }
            }
        }

        public static bool GetGroundCollision(this GroundCollisionComponent self)
        {
            return self.GroundCollision;
        }
    }
}