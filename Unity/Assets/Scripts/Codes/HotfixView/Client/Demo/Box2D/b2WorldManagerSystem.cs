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

        private static void Init(this b2WorldManager self)
        {
            self.Game = null;
            self.B2World?.Dispose();
            
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

        public static Body CreateBody(this b2WorldManager self, BodyDef def)
        {
            return self.B2World.World.CreateBody(def);
        }

        public static void DestroyBody(this b2WorldManager self, Body body)
        {
            self.B2World.World.DestroyBody(body);
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