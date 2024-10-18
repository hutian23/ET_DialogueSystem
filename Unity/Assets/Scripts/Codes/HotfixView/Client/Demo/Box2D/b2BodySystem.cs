using Box2DSharp.Testbed.Unity.Inspection;
using UnityEngine;
using Transform = Box2DSharp.Common.Transform;

namespace ET.Client
{
    [FriendOf(typeof (b2Body))]
    [FriendOf(typeof (RootMotionComponent))]
    public static class b2BodySystem
    {
        public class b2BodyDestroySystem: DestroySystem<b2Body>
        {
            protected override void Destroy(b2Body self)
            {
                self.unitId = 0;
                self.fixtures.Clear();
                self.body = null;
                self.Flip = FlipState.Left;
                self.UpdateFlag = false;
            }
        }
        
        public static void SyncUnitTransform(this b2Body self)
        {
            Unit unit = Root.Instance.Get(self.unitId) as Unit;

            Transform curTrans = self.body.GetTransform();
            if (self.trans.Equals(curTrans) && !self.UpdateFlag)
            {
                return;
            }

            //同步渲染层GameObject和逻辑层b2World中刚体的位置旋转信息
            self.trans = curTrans;
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            Vector2 position = curTrans.Position.ToUnityVector2();
            Vector3 axis = new(0, 0, curTrans.Rotation.Angle * Mathf.Rad2Deg);

            go.transform.position = position;
            go.transform.eulerAngles = axis;
            go.transform.localScale = new Vector3(self.GetFlip(), 1, 1);

            //更新转向信息后，夹具也需要更新(质心镜像翻转了)
            if (self.UpdateFlag)
            {
                EventSystem.Instance.Invoke(new UpdateFlipCallback() { instanceId = self.unitId });
            }
            self.UpdateFlag = false;
        }

        public static void SetVelocityX(this b2Body self, float velocityX)
        {
            var oldVel = self.body.LinearVelocity;
            var newVel = new System.Numerics.Vector2(-velocityX * self.GetFlip(), oldVel.Y);
            self.body.SetLinearVelocity(newVel);
        }

        public static void SetFlip(this b2Body self, FlipState flipState)
        {
            self.Flip = flipState;
        }

        public static int GetFlip(this b2Body self)
        {
            return (int)self.Flip;
        }

        public static void SetUpdateFlag(this b2Body self)
        {
            self.UpdateFlag = true;
        }

        public static void RemoveUpdateFlag(this b2Body self)
        {
            self.UpdateFlag = false;
        }
    }
}