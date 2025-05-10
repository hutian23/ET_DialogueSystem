using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using Timeline;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public static class b2BodySystem
    {
        [FriendOf(typeof(b2WorldManager))]
        public class b2BodyDestroySystem : DestroySystem<b2Body>
        {
            protected override void Destroy(b2Body self)
            {
                self.unitId = 0;
                
                self.body = null;
                self.Fixtures.Clear();
                self.FixtureDict.Clear();

                self.Flip = FlipState.Left;
                self.VelocityX = 0f;
                self.VelocityY = 0f;
                self.Hertz = 60;

                self.TriggerEnterBuffer.Clear();
                self.TriggerStayBuffer.Clear();
                self.TriggerExitBuffer.Clear();
                self.CollisionEnterBuffer.Clear();
                self.CollisionStayBuffer.Clear();
                self.CollisionExitBuffer.Clear();
            }
        }

        public class B2bodyPreStepSystem : PreStepSystem<b2Body>
        {
            protected override void PreStepUpdate(b2Body self)
            {
                self.SetLinearVelocity(new Vector2(-self.VelocityX, self.VelocityY));
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

        public class B2bodyLateUpdateSystem : FrameLateUpdateSystem<b2Body>
        {
            protected override void FrameLateUpdate(b2Body self)
            {
                //2. 清空当前帧缓冲区
                self.TriggerEnterBuffer.Clear();
                self.TriggerStayBuffer.Clear();
                self.TriggerExitBuffer.Clear();
                
                self.CollisionEnterBuffer.Clear();
                self.CollisionStayBuffer.Clear();
                self.CollisionExitBuffer.Clear();
            }
        }
        
        private static void SyncTrans(this b2Body self)
        {
            Unit unit = Root.Instance.Get(self.unitId) as Unit;
            UnityEngine.GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;

            Transform trans = self.body.GetTransform();
            go.transform.position = trans.Position.ToUnityVector3();
            go.transform.eulerAngles = new UnityEngine.Vector3(0, 0, trans.Rotation.Angle * UnityEngine.Mathf.Rad2Deg);
            go.transform.localScale = new UnityEngine.Vector3(self.GetFlip(), 1, 1);
        }

        #region Velocity
        //真实速度 = 当前帧速度 * 朝向 * TimeScale
        public static void SetLinearVelocity(this b2Body self, Vector2 velocity)
        {
            Vector2 realVelocity = velocity * (self.Hertz / 60f) * new Vector2(self.GetFlip(), 1);
            self.body.SetLinearVelocity(realVelocity);
        }
        
        public static Vector2 GetVelocity(this b2Body self)
        {
            return new Vector2(self.VelocityX, self.VelocityY);
        }

        public static void SetVelocity(this b2Body self, Vector2 value)
        {
            //真实速度 = 当前帧速度 * 朝向 * TimeScale
            self.VelocityX = value.X;
            self.VelocityY = value.Y;
        }
        
        public static void SetVelocityY(this b2Body self, float velocityY)
        {
            self.SetVelocity(new Vector2(self.VelocityX, velocityY));
        }

        public static void SetVelocityX(this b2Body self, float velocityX)
        {
            self.SetVelocity(new Vector2(velocityX, self.VelocityY));
        }
        #endregion

        /// <summary>
        /// 激活刚体
        /// 刚体处于未激活状态下，不会参与碰撞、射线检测、查询
        /// 未激活刚体仍然可以创建夹具、关节
        /// </summary>
        public static void SetEnable(this b2Body self, bool isEnable)
        {
            self.body.IsEnabled = isEnable;
        }

        #region Flip
        /// <summary>
        /// 设置刚体朝向，渲染层同步朝向
        /// </summary>
        public static void SetFlip(this b2Body self, FlipState flipState)
        {
            if ((int)flipState == self.GetFlip()) return;
            self.Flip = flipState;
            
            //1. 获取类型为Hitbox的夹具
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
            self.ClearFixtures(FixtureType.Hitbox);

            //2. 水平翻转夹具
            int count = dataQueue.Count;
            while (count -- > 0)
            {
                FixtureData data = dataQueue.Dequeue();
                if (data.UserData is not BoxInfo info)
                {
                    continue;
                }
                
                //3. 实际上，转向需要重新创建夹具
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
            
            //3. 渲染层同步朝向
            self.SyncTrans();
        }

        public static int GetFlip(this b2Body self)
        {
            return (int)self.Flip;
        }
        #endregion

        #region Position
        public static void SetPosition(this b2Body self, Vector2 position)
        {
            self.body.SetTransform(position, 0f);
        }

        public static Vector2 GetPosition(this b2Body self)
        {
            return self.body.GetPosition();
        }
        #endregion

        #region Hertz
        public static int GetHertz(this b2Body self)
        {
            return self.Hertz;
        }

        public static void SetHertz(this b2Body self, int hertz)
        {
            self.Hertz = hertz;
        }
        #endregion
        
        #region Fixture
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
        
        /// <summary>
        /// 根据夹具类型移除夹具
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fixtureType">eg. FixtureType.Hitbox</param>
        public static void ClearFixtures(this b2Body self, int fixtureType)
        {
            QueueComponent<Fixture> removeQueue = QueueComponent<Fixture>.Create();
            
            //1. 找到指定类型的夹具
            for (int i = 0; i < self.Fixtures.Count; i++)
            {
                Fixture fixture = self.Fixtures[i];
                FixtureData data = (FixtureData)fixture.UserData;
                if (data.Type == fixtureType)
                {
                    removeQueue.Enqueue(fixture);
                }
            }
         
            //2. 移除
            int count = removeQueue.Count;
            while (count-- > 0)
            {
                Fixture fixture = removeQueue.Dequeue();
                self.DestroyFixture(fixture);
            }
            
            removeQueue.Dispose();
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