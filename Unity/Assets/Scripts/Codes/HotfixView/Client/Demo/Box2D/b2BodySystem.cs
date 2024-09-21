using Box2DSharp.Testbed.Unity.Inspection;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (b2Body))]
    public static class b2BodySystem
    {
        public class b2BodyLoadSystem: LoadSystem<b2Body>
        {
            protected override void Load(b2Body self)
            {
                self.Dispose();
            }
        }

        public class b2BodyDestroySystem: DestroySystem<b2Body>
        {
            protected override void Destroy(b2Body self)
            {
                self.unitId = 0;
                self.IsPlayer = false;
                self.body = null;
            }
        }

        public static void SyncUnitTransform(this b2Body self)
        {
            Unit unit = TODUnitHelper.GetPlayer(self.ClientScene());

            var curTrans = self.body.GetTransform();
            if (self.trans.Equals(curTrans))
            {
                return;
            }

            self.trans = curTrans;

            //同步渲染层GameObject和逻辑层b2World中刚体的位置旋转信息
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            go.transform.position = curTrans.Position.ToUnityVector2();
            go.transform.eulerAngles = new Vector3(0, 0, curTrans.Rotation.Angle * Mathf.Rad2Deg);
        }
    }
}