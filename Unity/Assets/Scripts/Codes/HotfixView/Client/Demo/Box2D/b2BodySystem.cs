using Box2DSharp.Common;
using Box2DSharp.Testbed.Unity.Inspection;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    [FriendOf(typeof (b2Body))]
    public static class b2BodySystem
    {
        public class b2BodyDestroySystem: DestroySystem<b2Body>
        {
            protected override void Destroy(b2Body self)
            {
                self.unitId = 0;
                self.fixtures.Clear();
                self.body = null;
                self.velocity = Vector2.Zero;
                self.frame = 0;
            }
        }

        public static void SyncUnitTransform(this b2Body self)
        {
            Unit unit = Root.Instance.Get(self.unitId) as Unit;

            var curTrans = self.body.GetTransform();
            if (self.trans.Equals(curTrans))
            {
                return;
            }

            self.trans = curTrans;

            //同步渲染层GameObject和逻辑层b2World中刚体的位置旋转信息
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            var position = curTrans.Position.ToUnityVector2();
            var axis = MathUtils.Mul(curTrans.Rotation, new System.Numerics.Vector2(1.0f, 0.0f)).ToUnityVector2();

            go.transform.position = position;
            go.transform.eulerAngles = axis;
        }
    }
}