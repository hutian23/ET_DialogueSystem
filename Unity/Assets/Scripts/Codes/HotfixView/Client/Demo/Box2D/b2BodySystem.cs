using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Timeline;
using Transform = Box2DSharp.Common.Transform;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public static class b2BodySystem
    {
        public class b2BodyDestroySystem: DestroySystem<b2Body>
        {
            protected override void Destroy(b2Body self)
            {
                self.body = null;
                self.unitId = 0;
                self.Fixtures.Clear();
                self.FixtureDict.Clear();
                self.trans = default;
                self.offset = Vector2.Zero;
                self.Flip = FlipState.Left;
                self.UpdateFlag = false;
            }
        }
        
        public class B2bodyPostStepSystem : PostStepSystem<b2Body>
        {
            protected override void PosStepUpdate(b2Body self)
            {
                //static body
                if (self.unitId == 0) return;
                
                //未发生更新，渲染层无需刷新
                Transform curTrans = self.body.GetTransform();
                if (self.trans.Equals(curTrans) && !self.UpdateFlag) return;
                self.UpdateFlag = false;
                
                // 渲染层同步逻辑层刚体的位置
                self.SyncTrans();
            }
        }

        private static void SyncTrans(this b2Body self)
        {
            //同步渲染层GameObject和逻辑层b2World中刚体的位置旋转信息
            self.trans = self.body.GetTransform();
            Unit unit = Root.Instance.Get(self.unitId) as Unit;
            UnityEngine.GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            
            Vector2 position = self.trans.Position + self.offset;
            go.transform.position = new UnityEngine.Vector3(position.X, position.Y);
            go.transform.eulerAngles = new UnityEngine.Vector3(0, 0, self.trans.Rotation.Angle * UnityEngine.Mathf.Rad2Deg);
            go.transform.localScale = new UnityEngine.Vector3(self.GetFlip(), 1, 1);
                
            self.offset = Vector2.Zero;
        }
        
        public static Vector2 GetVelocity(this b2Body self)
        {
            return self.body.LinearVelocity;
        }

        public static void SetVelocity(this b2Body self, Vector2 value)
        {
            self.body.SetLinearVelocity(value);
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
        
        /// <summary>
        /// 设置刚体朝向，渲染层同步朝向
        /// </summary>
        public static void SetFlip(this b2Body self, FlipState flipState)
        {
            if ((int)flipState == self.GetFlip()) return;
            self.Flip = flipState;
            
            //1. 翻转类型为hitbox的夹具
            QueueComponent<FixtureData> dataQueue = new QueueComponent<FixtureData>();
            foreach (Fixture fixture in self.Fixtures)
            {
                FixtureData data = (FixtureData)fixture.UserData;
                if (data.Type is not FixtureType.Hitbox)
                {
                    continue;
                }
                dataQueue.Enqueue(data);
            }
            self.ClearHitBoxes();

            int count = dataQueue.Count;
            while (count -- > 0)
            {
                FixtureData data = dataQueue.Dequeue();
                if (data.UserData is not BoxInfo info)
                {
                    continue;
                }
                //reset param of fixtureDef
                PolygonShape shape = new();
                shape.SetAsBox(info.size.x / 2, info.size.y / 2, new Vector2(info.center.x * self.GetFlip(), info.center.y), 0f);
                FixtureDef fixtureDef = new()
                {
                    Shape = shape,
                    Density = 1.0f,
                    Friction = 0.0f,
                    UserData = data
                };
                self.CreateFixture(fixtureDef);
            }
            dataQueue.Dispose();
            
            //2. 渲染层同步朝向
            Unit unit = Root.Instance.Get(self.unitId) as Unit;
            UnityEngine.GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            go.transform.localScale = new UnityEngine.Vector3(self.GetFlip(), 1, 1);
        }

        public static int GetFlip(this b2Body self)
        {
            return (int)self.Flip;
        }

        public static void SetPosition(this b2Body self, Vector2 position)
        {
            self.body.SetTransform(position, 0f);
        }

        public static Vector2 GetPosition(this b2Body self)
        {
            return self.body.GetPosition();
        }

        #region Fixture
        /// <summary>
        /// 取消行为时，销毁hitbox
        /// </summary>
        /// <param name="self"></param>
        public static void ClearHitBoxes(this b2Body self)
        {
            //销毁hitbox
            QueueComponent<Fixture> removeQueue = QueueComponent<Fixture>.Create();
            for (int i = 0; i < self.Fixtures.Count; i++)
            {
                Fixture fixture = self.Fixtures[i];
                FixtureData data = (FixtureData)fixture.UserData;
                if (data.Type is FixtureType.Hitbox)
                {
                    removeQueue.Enqueue(fixture);
                }
            }
            int count = removeQueue.Count;
            while (count-- > 0)
            {
                Fixture fixture = removeQueue.Dequeue();
                self.DestroyFixture(fixture);
            }
            removeQueue.Dispose();
        }
        
        /// <summary>
        /// 热重载时调用，销毁所有夹具
        /// </summary>
        /// <param name="self"></param>
        private static void ClearFixtures(this b2Body self)
        {
            if (b2WorldManager.Instance.IsLocked())
            {
                Log.Error($"cannot dispose fixture while b2World is locked!!");
                return;
            }
            for (int i = 0; i < self.Fixtures.Count; i++)
            {
                Fixture fixture = self.Fixtures[i];
                self.body.DestroyFixture(fixture);
            }
            self.Fixtures.Clear();
            self.FixtureDict.Clear();
        }

        private static void DestroyFixture(this b2Body self, string fixtureName)
        {
            if (b2WorldManager.Instance.IsLocked())
            {
                Log.Error($"cannot destroy fixture while b2World is locked!!");
                return;
            }

            if (!self.FixtureDict.TryGetValue(fixtureName, out Fixture fixture))
            {
                Log.Error($"not found fixture: {fixtureName}");
                return;
            }

            self.Fixtures.Remove(fixture);
            self.FixtureDict.Remove(fixtureName);
            self.body.DestroyFixture(fixture);
        }

        public static void DestroyFixture(this b2Body self, Fixture fixture)
        {
            FixtureData data = (FixtureData)fixture.UserData;
            self.DestroyFixture(data.Name);
        }
        
        public static Fixture CreateFixture(this b2Body self,FixtureDef fixtureDef)
        {
            if (b2WorldManager.Instance.IsLocked())
            {
                Log.Error($"cannot create fixture while b2World is locked!!");
                return null;
            }
            FixtureData data = (FixtureData)fixtureDef.UserData;
            if (string.IsNullOrEmpty(data.Name))
            {
                Log.Error($"fixture name should not be null or empty!!");
                return null;
            }
            if (self.FixtureDict.ContainsKey(data.Name))
            {
                Log.Error($"already contain fixture!, name: {data.Name}");
                return null;
            }
            
            Fixture fixture = self.body.CreateFixture(fixtureDef);
            self.Fixtures.Add(fixture);
            self.FixtureDict.Add(data.Name, fixture);

            return fixture;
        }

        public static Fixture GetFixture(this b2Body self, string name)
        {
            if (!self.FixtureDict.TryGetValue(name, out Fixture fixture))
            {
                Log.Error($"not found fixture: {name}");
            }
            return fixture;
        }
        #endregion
    }
}