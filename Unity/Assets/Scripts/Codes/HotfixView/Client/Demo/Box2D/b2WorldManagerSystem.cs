using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Common;
using Box2DSharp.Dynamics;
using ET.Event;
using Testbed.Abstractions;
using Camera = UnityEngine.Camera;

namespace ET.Client
{
    [FriendOf(typeof (b2WorldManager))]
    [FriendOf(typeof (b2Body))]
    public static class b2WorldManagerSystem
    {
        public class b2WorldManagerAwakeSystem: AwakeSystem<b2WorldManager>
        {
            protected override void Awake(b2WorldManager self)
            {
                b2WorldManager.Instance = self;
                self.PreStepTimer = self.AddChild<BBTimerComponent>().Id;
                self.PostStepTimer = self.AddChild<BBTimerComponent>().Id;
                self.GizmosTimer = self.AddChild<BBTimerComponent>().Id;
                self.Reload();
            }
        }

        public class b2WorldManagerDestroySystem: DestroySystem<b2WorldManager>
        {
            protected override void Destroy(b2WorldManager self)
            {
                b2WorldManager.Instance = null;
                self.Init();
            }
        }
        
        public class b2WorldManagerLoadSystem : LoadSystem<b2WorldManager>
        {
            protected override void Load(b2WorldManager self)
            {
                self.Reload();
            }
        }

        public class b2WorldManagerPreStepSystem : PreStepSystem<b2WorldManager>
        {
            protected override void PreStepUpdate(b2WorldManager self)
            {
                int count = self.DisposeQueue.Count;
                while (count-- > 0)
                {
                    Body body = self.DisposeQueue.Dequeue();
                    self.B2World.World.DestroyBody(body);
                }
            }
        }
        
        public class b2WorldManagerGizmosUpdateSystem : GizmosUpdateSystem<b2WorldManager>
        {
            protected override void GizmosUpdate(b2WorldManager self)
            {
                self.GetGizmosTimer().Step();
            }
        }

        private static void Init(this b2WorldManager self)
        {
            self.Game = null;
            self.B2World?.Dispose();

            foreach (var kv in self.BodyDict)
            {
                b2Body body = Root.Instance.Get(kv.Value) as b2Body;
                body.Dispose();
            }
            self.BodyDict.Clear();
            self.DisposeQueue.Clear();
            
            //reload timer
            BBTimerComponent PreStepTimer = self.GetChild<BBTimerComponent>(self.PreStepTimer);
            BBTimerComponent PostStepTimer = self.GetChild<BBTimerComponent>(self.PostStepTimer);
            BBTimerComponent GizmosTimer = self.GetChild<BBTimerComponent>(self.GizmosTimer);
            PreStepTimer.Reload();
            PostStepTimer.Reload();
            GizmosTimer.Reload();
            
            Global.Settings.Pause = false;
            Global.Settings.SingleStep = false;
        }
        
        private static void Reload(this b2WorldManager self)
        {
            self.Init();
            self.Game = Camera.main.GetComponent<b2Game>();
            self.B2World = new b2World(self.Game);
            EventSystem.Instance.PublishAsync(self.DomainScene(), new AfterB2WorldCreated() { B2World = self.B2World }).Coroutine();
        }

        #region B2body

        public static b2Body CreateBody(this b2WorldManager self, long unitId, BodyDef bodyDef)
        {
            if (self.BodyDict.ContainsKey(unitId))
            {
                Log.Error($"already exist b2body with unitId: {unitId}");
                return null;
            }

            b2Body b2Body = b2WorldManager.Instance.AddChild<b2Body>();
            b2Body.body = b2WorldManager.Instance.B2World.World.CreateBody(bodyDef);
            b2Body.unitId = unitId;
            
            self.BodyDict.Add(b2Body.unitId, b2Body.InstanceId);
            return b2Body;
        }
        
        /// <summary>
        /// 通过Unit.InstanceId查找对应的b2Body
        /// </summary>
        public static b2Body GetBody(this b2WorldManager self, long unitId)
        {
            if (!self.BodyDict.TryGetValue(unitId, out long instanceId))
            {
                Log.Error($"does not exist b2Body, unit.InstanceId: {unitId}");
                return null;
            }
            
            return Root.Instance.Get(instanceId) as b2Body;
        }

        /// <summary>
        /// 通过Unit.InstanceId删除对应b2Body
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unitId"></param>
        public static void DestroyBody(this b2WorldManager self, long unitId)
        {
            if (!self.BodyDict.TryGetValue(unitId, out long instanceId))
            {
                Log.Error($"does not exist b2Body, unit.InstanceId: {unitId}");
                return;
            }
            
            b2Body b2Body = Root.Instance.Get(instanceId) as b2Body;
            b2Body.Dispose();
            
            self.BodyDict.Remove(unitId);
        }
        
        /// <summary>
        /// 激活刚体
        /// </summary>
        public static void EnableBody(this b2WorldManager self, long instanceId,bool enable)
        {
            if (!self.BodyDict.ContainsKey(instanceId))
            {
                return;
            }
            b2Body b2Body = self.GetBody(instanceId);
            b2Body.SetEnable(enable);
        }
        
        #endregion
        
        public static void Step(this b2WorldManager self)
        {
            self.B2World.Step();
        }
        
        public static BBTimerComponent GetPreStepTimer(this b2WorldManager self)
        {
            return self.GetChild<BBTimerComponent>(self.PreStepTimer);
        }

        public static BBTimerComponent GetPostStepTimer(this b2WorldManager self)
        {
            return self.GetChild<BBTimerComponent>(self.PostStepTimer);
        }

        public static BBTimerComponent GetGizmosTimer(this b2WorldManager self)
        {
            return self.GetChild<BBTimerComponent>(self.GizmosTimer);
        }
        
        public static bool IsLocked(this b2WorldManager self)
        {
            return self.B2World.IsLocked;
        }

        public static void DrawShape(this b2WorldManager self, Shape shape, Vector2 position, float angle, Color color)
        {
            self.B2World.DrawShape(shape, position, angle, color);
        }

        public static void DrawPoint(this b2WorldManager self, Vector2 point, float size, Color color)
        {
            self.B2World.Draw.DrawPoint(point, size, color);
        }

        public static void DrawSegment(this b2WorldManager self, Vector2 start, Vector2 end, Color color)
        {
            self.B2World.Draw.DrawSegment(start, end, color);
        }
    }
}