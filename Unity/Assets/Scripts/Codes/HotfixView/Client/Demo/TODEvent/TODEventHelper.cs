using System;

namespace ET.Client
{
    [FriendOf(typeof (TODEventSystem))]
    public static class TODEventHelper
    {
        public class TODEventAwakeSystem : AwakeSystem<TODEventSystem>
        {
            protected override void Awake(TODEventSystem self)
            {
                TODEventSystem.Instance = self;
                self.Init();
            }
        }
        
        public class TODEventDestorySystem : DestroySystem<TODEventSystem>
        {
            protected override void Destroy(TODEventSystem self)
            {
                self.Behaviors.Clear();
                self.checkers.Clear();
                TODEventSystem.Instance = null;
            }
        }

        public class TODEventLoadSystem: LoadSystem<TODEventSystem>
        {
            protected override void Load(TODEventSystem self)
            {
                self.Init();
            }
        }
        
        private static void Init(this TODEventSystem self)
        {
            //1. 加载BehaviorHandler
            self.Behaviors.Clear();
            var BehaviorTypes = EventSystem.Instance.GetTypes(typeof (BehaviorAttribute));
            Log.Debug($"一共有 {BehaviorTypes.Count} 个BehaviroHandler");
            foreach (Type type in BehaviorTypes)
            {
                BehaviorHandler behaviorHandler = Activator.CreateInstance(type) as BehaviorHandler;
                if (behaviorHandler == null)
                {
                    Log.Error($"this behavior is not a BehaviorHandler!: {type.Name}");
                    continue;
                }

                self.Behaviors.Add(type.Name, behaviorHandler);
            }

            //2. 加载CheckerHandler
            self.checkers.Clear();
            var checkerTypes = EventSystem.Instance.GetTypes(typeof (CheckerAttribute));
            Log.Debug($"一共有 {checkerTypes.Count} 个checkerHandler");
            foreach (Type type in checkerTypes)
            {
                CheckerHandler checkerHandler = Activator.CreateInstance(type) as CheckerHandler;
                if (checkerHandler == null)
                {
                    Log.Error($"this behavior is not a behaviorHandler!: {type.Name}");
                    continue;
                }

                self.checkers.Add(type.Name, checkerHandler);
            }
        }

        public static BehaviorHandler GetBehavior(this TODEventSystem self, string name)
        {
            self.Behaviors.TryGetValue(name, out var handler);
            if (handler == null)
            {
                Log.Error($"不存在behavior: {name}");
            }

            return handler;
        }

        public static CheckerHandler GetChecker(this TODEventSystem self, string name)
        {
            self.checkers.TryGetValue(name, out var handler);
            if (handler == null)
            {
                Log.Error($"不存在behavior: {name}");
            }

            return handler;
        }
    }
}