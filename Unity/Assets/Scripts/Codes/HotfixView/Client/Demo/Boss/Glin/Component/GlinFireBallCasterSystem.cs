using System;
using System.Numerics;

namespace ET.Client
{
    [FriendOf(typeof(GlinFireBallCaster))]
    public static class GlinFireBallCaster_VerticalSystem
    {
        public class GlinFireballCaster_VerticalDestroySystem : DestroySystem<GlinFireBallCaster>
        {
            protected override void Destroy(GlinFireBallCaster self)
            {
                self.lastFrame = 0;
                self.waitFrame = 0;
                self.startV = 0f;
                self.accelX = 0f;
                self.accelY = 0f;
                self.token.Cancel();
            }
        }

        public static void StartSpawnCor(this GlinFireBallCaster self, int lastFrame, int waitFrame, float startV, float accelX, float accelY)
        {
            self.lastFrame = lastFrame;
            self.waitFrame = waitFrame;
            self.startV = startV;
            self.accelX = accelX;
            self.accelY = accelY;
            self.token = new ETCancellationToken();
            self.LastCor().Coroutine();
            self.SpawnCor().Coroutine();
        }

        private static async ETTask LastCor(this GlinFireBallCaster self)
        {
            Unit caster = self.GetParent<BBParser>().GetParent<Unit>();
            BBTimerComponent bbTimer = caster.GetComponent<BBTimerComponent>();

            await bbTimer.WaitAsync(self.lastFrame, self.token);
            if (self.token.IsCancel()) return;
            
            self.Dispose();
        }
        
        private static async ETTask SpawnCor(this GlinFireBallCaster self)
        {
            Unit caster = self.GetParent<BBParser>().GetParent<Unit>();
            BBTimerComponent bbTimer = caster.GetComponent<BBTimerComponent>();

            int cnt = 200;
            while (cnt-- > 0)
            {
                //2. 纵向飞弹
                self.SpawnVerticalFireball();
                
                //3. 横向飞弹
                self.SpawnHorizontalFireball();
                
                await bbTimer.WaitAsync(self.waitFrame, self.token);
                if (self.token.IsCancel()) return;
            }

            await ETTask.CompletedTask;
        }

        private static void SpawnVerticalFireball(this GlinFireBallCaster self)
        {
            Unit caster = self.GetParent<BBParser>().GetParent<Unit>();
            
            for (int i = 0; i < 3; i++)
            {
                //1. 创建bullet
                Unit bullet = BattleSceneManager.Instance.AddChild<Unit, int>(1001);
                bullet.AddComponent<GameObjectComponent>().GameObject = GameObjectPoolHelper.GetObjectFromPool("GlinFireball");
                bullet.AddComponent<BBParser>();
                b2Body bodyA = b2WorldManager.Instance.GetBody(caster.InstanceId);
                b2Body bodyB = b2WorldManager.Instance.GetBody(bullet.InstanceId);
                    
                //2-1 初始位置
                bodyB.SetPosition(bodyA.GetPosition() + new Vector2(i / 2 == 0? (2 * i - 1) * 0.9f : 0f, 0.8f));
                //2-2 初始速度
                bodyB.SetVelocityY(self.startV * new Random().Next(80, 120) / 100f);
                //2-3 飞弹发射后，逐渐向两边展开
                if (i / 2 == 0)
                {
                    float accelX = -self.accelX * (2 * i - 1);
                    bullet.GetComponent<BBParser>().AddComponent<AccelXComponent, float, float, int>(0, accelX, 100, true);
                }
            }
        }

        private static void SpawnHorizontalFireball(this GlinFireBallCaster self)
        {
            Unit caster = self.GetParent<BBParser>().GetParent<Unit>();

            for (int i = 0; i < 6; i++)
            {
                //1. 创建bullet
                Unit bullet = BattleSceneManager.Instance.AddChild<Unit, int>(1001);
                bullet.AddComponent<GameObjectComponent>().GameObject = GameObjectPoolHelper.GetObjectFromPool("GlinFireball");
                bullet.AddComponent<BBParser>();

                //2. bullet初始数值
                b2Body bodyA = b2WorldManager.Instance.GetBody(caster.InstanceId);
                b2Body bodyB = b2WorldManager.Instance.GetBody(bullet.InstanceId);
                
                //2-1 初始位置
                bodyB.SetPosition(bodyA.GetPosition() + new Vector2(new Random().Next(-40, 40) / 100f, new Random().Next(-120, 120) / 100f));
                //2-2 初始速度
                float velX = (i / 3 == 0 ? 1: -1) * self.startV;
                float velY = i switch
                {
                    0 or 3 => 1,
                    1 or 4 => 0,
                    2 or 5 => -1,
                    _ => 0
                } * self.startV * 1.6f + new Random().Next(-20, 20) / 100f * self.startV;
                bodyB.SetVelocity(new Vector2(velX, velY));
                
                //3. 
                bullet.GetComponent<BBParser>().AddComponent<GlinFireballAccel, float, float>(velY, self.accelY);
            }
        }
    }
}