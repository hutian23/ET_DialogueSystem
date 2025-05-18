using System.Collections.Generic;
using ET.Event;
using Timeline;

namespace ET.Client
{
    public static class GroundCollisionCallbackSystem
    {
        public class GroundCollisionCallbackDestroySystem : DestroySystem<GroundCollisionCallback>
        {
            protected override void Destroy(GroundCollisionCallback self)
            {
                self.functionIndex = 0;
                self.info = default;
            }
        }

        [FriendOf(typeof(b2Body))]
        [FriendOf(typeof(BBParser))]
        public class GroundCollisionCallbackPostStepSystem : PostStepSystem<GroundCollisionCallback>
        {
            protected override void PosStepUpdate(GroundCollisionCallback self)
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

                    // 地面碰撞回调
                    self.info = info;
                    parser.Invoke(self.functionIndex, parser.CancellationToken).Coroutine();
                    self.info = default;
                    
                    return;
                }
            }
        }
    }
}