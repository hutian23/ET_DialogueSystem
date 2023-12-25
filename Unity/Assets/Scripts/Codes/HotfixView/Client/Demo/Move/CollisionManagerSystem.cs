using UnityEngine;

namespace ET.Client
{
    public static class CollisionManagerSystem
    {
        public class CollisionManagerAwakeSystem: AwakeSystem<CollisionManager>
        {
            protected override void Awake(CollisionManager self)
            {
                CollisionManager.Instance = self;
            }
        }

        public class CollisionManagerDestroySystem: DestroySystem<CollisionManager>
        {
            protected override void Destroy(CollisionManager self)
            {
                self.Actors.Clear();
                self.Solids.Clear();
            }
        }

        [FriendOf(typeof (TODMoveComponent))]
        public class CollisionManagerFixedUpdateSystem: FixedUpdateSystem<CollisionManager>
        {
            protected override void FixedUpdate(CollisionManager self)
            {
                int count = self.Actors.Count;
                while (count-- > 0)
                {
                    long instanceId = self.Actors.Dequeue();
                    TODMoveComponent moveComponent = Root.Instance.Get(instanceId) as TODMoveComponent;
                    if (moveComponent == null || moveComponent.IsDisposed)
                    {
                        continue;
                    }

                    if (!moveComponent.IsActor)
                    {
                        continue;
                    }

                    moveComponent.actorHandler.UpdateCollideX(moveComponent, moveComponent.Speed.x * Time.fixedDeltaTime);
                    moveComponent.actorHandler.UpdateCollideY(moveComponent, moveComponent.Speed.y * Time.fixedDeltaTime);
                    moveComponent.GetTransform().position = moveComponent.position;
                    
                    self.Actors.Enqueue(instanceId);
                }

                // count = self.Solids.Count;
                // while (count-- > 0)
                // {
                //     long instanceId = self.Solids.Dequeue();
                //     TODMoveComponent moveComponent = Root.Instance.Get(instanceId) as TODMoveComponent;
                //     if (moveComponent == null || moveComponent.IsDisposed)
                //     {
                //         continue;
                //     }
                //
                //     if (moveComponent.IsActor)
                //     {
                //         continue;
                //     }
                //     
                //     // moveComponent.solidHandler.UpdateCollideX();
                // }
            }
        }
    }
}