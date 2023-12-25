namespace ET.Client
{
    public static class CollisionManagerSystem
    {
        public class  CollisionManagerDestroySystem : DestroySystem<CollisionManager>
        {
            protected override void Destroy(CollisionManager self)
            {
                self.Actors.Clear();
                self.Solids.Clear();
            }
        }
        
        [FriendOf(typeof(TODMoveComponent))]
        public class CollisionManagerUpdateSystem : UpdateSystem<CollisionManager>
        {
            protected override void Update(CollisionManager self)
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
                    
                    // moveComponent.actorHandler.UpdateCollideX();
                    self.Actors.Enqueue(instanceId);
                }
            }
        }
    }
}