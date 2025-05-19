using System;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    [FriendOf(typeof(GlinSpikeCaster))]
    public static class GlinSpikeCasterSystem
    {
        public class GlinSpikeManagerDestroySystem : DestroySystem<GlinSpikeCaster>
        {
            protected override void Destroy(GlinSpikeCaster self)
            {
                self.waitFrame = 0;
                self.spawnCount = 0;
                self.token.Cancel();
            }
        }

        public static void StartSpawnCor(this GlinSpikeCaster self, int spawnCount, int waitFrame)
        {
            self.waitFrame = waitFrame;
            self.spawnCount = spawnCount;
            self.token = new ETCancellationToken();
            self.SpawnCor().Coroutine();
        }
        
        private static async ETTask SpawnCor(this GlinSpikeCaster self)
        {
            Unit unit = self.GetParent<BBParser>().GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();

            while (self.spawnCount-- > 0)
            {
                //1. 等待n帧
                await bbTimer.WaitAsync(self.waitFrame, self.token);
                if (self.token.IsCancel()) return;
                
                //2. 玩家附近生成三根地刺
                Unit player = BBUnitHelper.GetPlayer(self.ClientScene());
                float x = new Random().Next(-100, 100) / 100f * 2f;
                for (int i = 0; i < 3; i++)
                {
                    Unit bullet = BulletManager.Instance.AddChild<Unit, int>(1001);
                    bullet.AddComponent<GameObjectComponent>().GameObject = GameObjectPoolHelper.GetObjectFromPool("GlinSpike");
                    bullet.AddComponent<BBParser>();   
                    
                    b2Body bodyA = b2WorldManager.Instance.GetBody(player.InstanceId);
                    b2Body bodyB = b2WorldManager.Instance.GetBody(bullet.InstanceId);

                    float targetX = bodyA.GetPosition().X + (i - 1) * 1.5f + x;
                    bodyB.SetPosition(new Vector2(targetX, -11.5f));
                }
            }
            
            self.Dispose();
        }
    }
}