using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using ET.Event;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public static class b2BodySystem
    {
        public class B2bodyPreStepSystem : PreStepSystem<b2Body>
        {
            protected override void PreStepUpdate(b2Body self)
            {
                self.SetAngle(self.angle);
                self.SetLinearVelocity(self.velocity);
            }
        }
        
        public class B2bodyPostStepSystem : PostStepSystem<b2Body>
        {
            protected override void PosStepUpdate(b2Body self)
            {
                //1. 渲染层同步逻辑层刚体的位置
                self.SyncTrans();
            }
        }

        public class B2bodyFrameLateUpdateSystem : FrameLateUpdateSystem<b2Body>
        {
            protected override void FrameLateUpdate(b2Body self)
            {
                //2. 清空当前帧缓冲区
                self.triggerEnterBuffers.Clear();
                self.triggerStayBuffers.Clear();
                self.triggerExitBuffers.Clear();
                self.collisionEnterBuffers.Clear();
                self.collisionStayBuffers.Clear();
                self.collisionExitBuffers.Clear();
            }
        }
        
        [FriendOf(typeof(b2WorldManager))]
        public class b2BodyDestroySystem : DestroySystem<b2Body>
        {
            protected override void Destroy(b2Body self)
            {
                b2WorldManager.Instance.DestroyBody(self.body);
                self.body = null;
                self.unitId = 0;
                self.b2BoxDict.Clear();
                
                self.flip = FlipState.Left;
                self.angle = 0f;
                self.hertz = 60;
                self.velocity = Vector2.Zero;
                
                self.triggerEnterBuffers.Clear();
                self.triggerStayBuffers.Clear();
                self.triggerExitBuffers.Clear();
                self.collisionEnterBuffers.Clear();
                self.collisionStayBuffers.Clear();
                self.collisionExitBuffers.Clear();
            }
        }

        #region b2Box

        public static void DestroyFixture(this b2Body self, Fixture fixture)
        {
            if (b2WorldManager.Instance.IsLocked())
            {
                Log.Error($"cannot destroy fixture while b2World is locked!!");
                return;
            }
            self.body.DestroyFixture(fixture);
        }
        
        public static Fixture CreateFixture(this b2Body self,FixtureDef fixtureDef)
        {
            if (b2WorldManager.Instance.IsLocked())
            {
                Log.Error($"cannot create fixture while b2World is locked!!");
                return null;
            }
            return self.body.CreateFixture(fixtureDef);
        }
        
        /// <summary>
        /// 激活刚体
        /// 刚体处于未激活状态下，不会参与碰撞、射线检测、查询
        /// 未激活刚体仍然可以创建夹具、关节
        /// </summary>
        public static void SetEnable(this b2Body self, bool isEnable)
        {
            self.body.IsEnabled = isEnable;
        }

        
        private static bool ContainBox(this b2Body self, string boxName)
        {
            return self.b2BoxDict.ContainsKey(boxName);
        }
        
        public static b2Box GetBox(this b2Body self, string boxName)
        {
            if (!self.b2BoxDict.TryGetValue(boxName, out long id))
            {
                Log.Error($"cannot found b2Box. boxName: {boxName}");
                return null;
            }

            return self.GetChild<b2Box>(id);
        }

        public static b2Box AddBox(this b2Body self, string boxName)
        {
            if (self.ContainBox(boxName))
            {
                Log.Error($"already exist b2Box. boxName: {boxName} unit.instanceId: {self.unitId}");
                return null;
            }

            b2Box b2Box = self.AddChild<b2Box>(true);
            self.b2BoxDict.Add(boxName, b2Box.Id);

            return b2Box;
        }

        public static void DestroyBox(this b2Body self, string boxName)
        {
            if (!self.b2BoxDict.Remove(boxName, out long id))
            {
                Log.Error($"cannot found b2Box. boxName: {boxName}");
                return;
            }

            b2Box box = self.GetChild<b2Box>(id);
            box.Dispose();
        }

        /// <summary>
        /// 删除指定HitboxType的夹具
        /// </summary>
        /// <param name="self"></param>
        /// <param name="filter">HitboxType.Hit | HitboxType.Hurt </param>
        public static void DestroyBoxes(this b2Body self, HitboxType filter)
        {
            ListComponent<long> ids = ListComponent<long>.Create(); // 池化管理
            foreach (var kv in self.b2BoxDict)
            {
                b2Box box = self.GetChild<b2Box>(kv.Value);
                if ((box.GetBoxType() & filter) != 0)
                {
                    ids.Add(box.Id); // note: 不能在集合内删除元素
                }
            }

            foreach (long id in ids)
            {
                b2Box box = self.GetChild<b2Box>(id);
                self.b2BoxDict.Remove(box.GetBoxName());
                box.Dispose();
            }
            ids.Dispose();
        }
        #endregion
        
        #region Flip
        /// <summary>
        /// 设置刚体朝向，渲染层同步朝向
        /// </summary>
        public static void SetFlip(this b2Body self, FlipState flipState)
        {
            self.SetFlip((int)flipState);
        }

        public static void SetFlip(this b2Body self, int flip)
        {
            EventSystem.Instance.Invoke(new UpdateFlipCallback(){instanceId = self.InstanceId, flip = flip});   
        }

        public static int GetFlip(this b2Body self)
        {
            return (int)self.flip;
        }
        
        #endregion
        
        #region Angle

        public static void SetAngle(this b2Body self, float angle)
        {
            self.angle = angle;
            self.body.SetTransform(self.GetPosition(), self.angle * UnityEngine.Mathf.Deg2Rad);
            self.SyncTrans();
        }

        public static float GetAngle(this b2Body self)
        {
            return self.angle;
        }

        public static float GetRadian(this b2Body self)
        {
            return self.GetAngle() * UnityEngine.Mathf.Deg2Rad;
        }
        
        #endregion
        
        #region Hertz
        public static int GetHertz(this b2Body self)
        {
            return self.hertz;
        }

        public static void SetHertz(this b2Body self, int hertz)
        {
            self.hertz = hertz;
        }
        #endregion
        
        #region Velocity
        //真实速度 = 当前帧速度 * 朝向 * TimeScale
        public static void SetLinearVelocity(this b2Body self, Vector2 velocity)
        {
            Vector2 realVelocity = velocity * (self.hertz / 60f) * new Vector2(-self.GetFlip(), 1);
            self.velocity = velocity;
            self.body.SetLinearVelocity(realVelocity);
        }
        
        public static Vector2 GetVelocity(this b2Body self)
        {
            return self.velocity;
        }

        public static void SetVelocity(this b2Body self, Vector2 value)
        {
            self.velocity = value;
        }
        
        public static void SetVelocityY(this b2Body self, float velocityY)
        {
            self.velocity.Y = velocityY;
        }

        public static void SetVelocityX(this b2Body self, float velocityX)
        {
            self.velocity.X = velocityX;
        }
        #endregion
        
        #region Transform
        public static void SetPosition(this b2Body self, Vector2 position)
        {
            self.body.SetTransform(position, 0f);
        }

        public static Vector2 GetPosition(this b2Body self)
        {
            return self.body.GetPosition();
        }
        
        public static Transform GetTransform(this b2Body self)
        {
            return self.body.GetTransform();
        }
        
        public static void SyncTrans(this b2Body self)
        {
            Unit unit = Root.Instance.Get(self.unitId) as Unit;
            UnityEngine.GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;

            Transform trans = self.body.GetTransform();
            go.transform.position = trans.Position.ToUnityVector3();
            go.transform.eulerAngles = new UnityEngine.Vector3(0, 0, trans.Rotation.Angle * UnityEngine.Mathf.Rad2Deg);
            go.transform.localScale = new UnityEngine.Vector3(self.GetFlip(), 1, 1);
        }
        #endregion
    }
}