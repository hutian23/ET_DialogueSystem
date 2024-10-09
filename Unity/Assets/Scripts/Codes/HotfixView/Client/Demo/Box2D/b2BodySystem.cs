using Box2DSharp.Testbed.Unity.Inspection;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (b2Body))]
    [FriendOf(typeof (RootMotionComponent))]
    public static class b2BodySystem
    {
        public class b2BodyAwakeSystem: AwakeSystem<b2Body>
        {
            protected override void Awake(b2Body self)
            {
            }
        }

        public class b2BodyDestroySystem: DestroySystem<b2Body>
        {
            protected override void Destroy(b2Body self)
            {
                self.unitId = 0;
                self.fixtures.Clear();
                self.body = null;
            }
        }

        public static void SyncVelocity(this b2Body self)
        {
            //根运动
            RootMotionComponent rootMotion = self.GetComponent<RootMotionComponent>();
            if (rootMotion == null)
            {
                return;
            }

            self.body.SetLinearVelocity(rootMotion.velocity);
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
            var axis = new Vector3(0, 0, curTrans.Rotation.Angle * Mathf.Rad2Deg);
            
            go.transform.position = position;
            go.transform.eulerAngles = axis;
        }

        public static void SetVelocityX(this b2Body self, float velocityX)
        {
            var oldVel = self.body.LinearVelocity;
            var newVel = new System.Numerics.Vector2(velocityX, oldVel.Y);
            self.body.SetLinearVelocity(newVel);
        }
    }
}