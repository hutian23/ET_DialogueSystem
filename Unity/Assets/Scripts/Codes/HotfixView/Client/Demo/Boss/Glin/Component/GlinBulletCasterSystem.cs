using Cinemachine;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    [FriendOf(typeof(GlinBulletCaster))]
    [FriendOf(typeof(ScreenShakeComponent))]
    public static class GlinBulletCasterSystem
    {
        public class GlinBulletCasterDestroySystem : DestroySystem<GlinBulletCaster>
        {
            protected override void Destroy(GlinBulletCaster self)
            {
                self.interval = 0;
                self.targetPos = Vector2.Zero;
                self.cnt = 0;
                self.offset = 0;

                self.shakeLengthX = 0f;
                self.shakeLengthY = 0f;
                self.frequency = 0f;
                self.shakeFrame = 0;
                
                self.token.Cancel();
            }
        }

        public static async ETTask CastCor(this GlinBulletCaster self)
        {
            Unit caster = self.GetParent<BBParser>().GetParent<Unit>();
            BBTimerComponent bbTimer = caster.GetComponent<BBTimerComponent>();

            for (int i = 0; i < self.cnt; i++)
            {
                //1. 创建 bullet unit
                Unit bullet = BulletManager.Instance.AddChild<Unit, int>(1001);
                GameObject go = GameObjectPoolHelper.GetObjectFromPool("GlinBullet");
                bullet.AddComponent<GameObjectComponent>().GameObject = go;
                bullet.AddComponent<BBParser>();

                //2. bullet 初始位置
                b2Body bodyA = b2WorldManager.Instance.GetBody(caster.InstanceId);
                b2Body bodyB = b2WorldManager.Instance.GetBody(bullet.InstanceId);

                Vector2 bulletPos = self.targetPos - new Vector2(0, self.offset * i);
                bodyB.SetPosition(bulletPos);
                bodyB.SetFlip((FlipState)(-bodyA.GetFlip()));

                //3. 产生振动
                VirtualCameraManager.Instance.RemoveComponent<ScreenShakeComponent>();
                ScreenShakeComponent screenShake = VirtualCameraManager.Instance.AddComponent<ScreenShakeComponent>(true);
                screenShake.shakeLength_X = self.shakeLengthX;
                screenShake.shakeLength_Y = self.shakeLengthY;
                screenShake.frequency = self.frequency;
                screenShake.totalFrame = self.shakeFrame;
                screenShake.curFrame = self.shakeFrame;
                screenShake.activeCamera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject;
                
                //4. 等待n帧生成下一个
                await bbTimer.WaitAsync(self.interval, self.token);
                if (self.token.IsCancel()) return;
            }
        }
    }
}