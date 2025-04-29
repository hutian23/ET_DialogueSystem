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
                self.shakeLength_X = 0;
                self.shakeLength_Y = 0;
                self.frequency = 0;
                self.totalFrame = 0;
                self.curFrame = 0;
                self.unitId = 0;
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
                Vector2 shakeLength = new Vector2(self.shakeLength_X, self.shakeLength_Y);
                Vector2 noise = new Vector2(_ran.Next(60, 120), _ran.Next(60, 120)) / 100f;
                Vector2 frequency = new Vector2(Mathf.Cos(self.curFrame * self.frequency ) * (self.curFrame / (float)self.totalFrame), Mathf.Sin(self.curFrame * self.frequency ) * (self.curFrame / (float)self.totalFrame));
                Vector2 shakePos = shakeLength * noise * frequency;
                
                //更新渲染层中gameObject位置
                go.transform.position += shakePos.ToUnityVector3();
            }
        }
    }
}