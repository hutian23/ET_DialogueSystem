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
        
        public class b2WorldManagerGizmosUpdateSystem : GizmosUpdateSystem<b2WorldManager>
        {
            protected override void GizmosUpdate(b2WorldManager self)
            {
                self.GetGizmosTimer().Step();
            }
        }
        
        public class b2WorldManagerPreStepSystem : PostStepSystem<b2WorldManager>
        {
            protected override void PosStepUpdate(b2WorldManager self)
            {
                b2WorldManager.Instance.GetPreStepTimer().Step();
            }
        }
        
        public class b2WorldManagerPostStepSystem : PostStepSystem<b2WorldManager>
        {
            protected override void PosStepUpdate(b2WorldManager self)
            {
                b2WorldManager.Instance.GetPostStepTimer().Step();
            }
        }

        private static void Init(this b2WorldManager self)
        {
            // 销毁子Entity
            foreach (long id in self.BodyDict.Values)
            {
                b2Body body = self.GetChild<b2Body>(id);
                body.Dispose();
            }
            self.BodyDict.Clear();
            
            // 销毁物理世界
            self.Game = null;
            self.B2World?.Dispose();
            
            // Editor相关
            Global.Settings.Pause = false;
            Global.Settings.SingleStep = false;
        }
        
        private static void Reload(this b2WorldManager self)
        {
            //1. 初始化
            self.Init();

            //2. 新建物理世界
            self.Game = Camera.main.GetComponent<b2Game>();
            self.B2World = new b2World(self.Game);
            EventSystem.Instance.PublishAsync(self.DomainScene(), new AfterB2WorldCreated() { B2World = self.B2World }).Coroutine();
            
            //3. 生命周期
            self.GetPreStepTimer().Reload();
            self.GetPostStepTimer().Reload();
            self.GetGizmosTimer().Reload();
        }

        public static b2Body CreateBody(this b2WorldManager self, long unitId, BodyDef bodyDef)
        {
            if (self.BodyDict.ContainsKey(unitId))
            {
                Log.Error($"already exist b2Body, unit.InstanceId: {unitId}");
                return null;
            }

            b2Body b2Body = self.AddChild<b2Body>();
            b2Body.unitId = unitId;
            b2Body.body = b2WorldManager.Instance.B2World.World.CreateBody(bodyDef);
            self.BodyDict.TryAdd(unitId, b2Body.Id);

            return b2Body;
        }

        public static void DestroyBody(this b2WorldManager self, long unitId)
        {
            if (!self.BodyDict.TryGetValue(unitId, out long id))
            {
                Log.Warning($"does not exist b2Body, unit.InstanceId: {unitId}");
                return;
            }

            b2Body b2Body = self.GetChild<b2Body>(id);
            b2WorldManager.Instance.BodyDict.Remove(unitId);
            
            b2Body.Dispose();
        }

        public static void DestroyBody(this b2WorldManager self, Body body)
        {
            self.B2World.World.DestroyBody(body);
        }
        
        public static bool ContainBody(this b2WorldManager self, long unitId)
        {
            return self.BodyDict.ContainsKey(unitId);
        }
        
        public static b2Body GetBody(this b2WorldManager self, long unitId)
        {
            if (!self.BodyDict.TryGetValue(unitId, out long id))
            {
                Log.Error($"cannot found b2Body, unit.InstanceId: {unitId}");
                return null;
            }

            return self.GetChild<b2Body>(id);
        }
        
        
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

        public static void Raycast(this b2WorldManager self, IRayCastCallback callback, Vector2 point1, Vector2 point2)
        {
            self.B2World.World.RayCast(callback, point1, point2);
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

        public static void DrawCircle(this b2WorldManager self, Vector2 position, float radius, Color color)
        {
            self.B2World.Draw.DrawCircle(position, radius, color);
        }
        
        //TODO 
        public static void DrawText(this b2WorldManager self, Vector2 position, string text, Color color)
        {
        }
    }
}