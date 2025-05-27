using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    [FriendOf(typeof(ShakeComponent))]
    public static class ShakeComponentSystem
    {
        public class ShakeComponentDestroySystem : DestroySystem<ShakeComponent>
        {
            protected override void Destroy(ShakeComponent self)
            {
                self.shakeLength = Vector2.Zero;
                self.frequency = 0;
                self.totalFrame = 0;
                self.curFrame = 0;
                self.unitId = 0;
                self.shakeMode = ShakeMode.Fading;
            }
        }
        
        public class ShakeComponentFrameLateUpdateSystem : FrameLateUpdateSystem<ShakeComponent>
        {
            protected override void FrameLateUpdate(ShakeComponent self)
            {
                Unit unit = Root.Instance.Get(self.unitId) as Unit;
                GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
                
                //shake效果结束
                if (self.curFrame-- < 0)
                {
                    self.Dispose();
                    return;
                }
                
                System.Random _ran = new();
                
                Vector2 noise = new Vector2(_ran.Next(60, 120), _ran.Next(60, 120)) / 100f;
                Vector2 frequency = new Vector2(Mathf.Cos(self.curFrame * self.frequency ), Mathf.Sin(self.curFrame * self.frequency ));
                Vector2 shakeLength = self.shakeLength * (self.shakeMode is ShakeMode.Fading ? (self.curFrame / (float)self.totalFrame) : 1); // 振动幅度是否随时间逐渐衰减?
                Vector2 shakePos = shakeLength * noise * frequency;
                
                //更新渲染层中gameObject位置
                go.transform.position += shakePos.ToUnityVector3();
            }
        }
    }
}