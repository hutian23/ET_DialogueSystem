using System;

namespace ET.Client
{
    [FriendOf(typeof (ScriptDispatcherComponent))]
    public static class ScriptDispatcherCompoentSystem
    {
        public class ScriptDispatcherAwakeSystem: AwakeSystem<ScriptDispatcherComponent>
        {
            protected override void Awake(ScriptDispatcherComponent self)
            {
                ScriptDispatcherComponent.Instance = self;
                self.Init();
            }
        }

        public class ScriptDispatcherLoadSystem: LoadSystem<ScriptDispatcherComponent>
        {
            protected override void Load(ScriptDispatcherComponent self)
            {
                self.Init();
            }
        }

        public class ScriptDispatcherDestroySystem: DestroySystem<ScriptDispatcherComponent>
        {
            protected override void Destroy(ScriptDispatcherComponent self)
            {
                self.ScriptHandlers.Clear();
                ScriptDispatcherComponent.Instance = null;
            }
        }

        private static void Init(this ScriptDispatcherComponent self)
        {
            self.ScriptHandlers.Clear();
            var scriptHandlers = EventSystem.Instance.GetTypes(typeof (ScriptAttribute));
            foreach (Type type in scriptHandlers)
            {
                ScriptHandler scriptHandler = Activator.CreateInstance(type) as ScriptHandler;
                if (scriptHandler == null)
                {
                    Log.Error($"this obj is not a scriptHandle:{type.Name}");
                    continue;
                }

                self.ScriptHandlers.Add(scriptHandler.GetOpType(), scriptHandler);
            }

            self.TriggerHandlers.Clear();
            var triggerHandlers = EventSystem.Instance.GetTypes(typeof (TriggerAttribute));
            foreach (var type in triggerHandlers)
            {
                TriggerHandler triggerHandler = Activator.CreateInstance(type) as TriggerHandler;
                if (triggerHandler == null)
                {
                    Log.Error($"this obj is not triggerHandler:{type.Name}");
                    continue;
                }

                self.TriggerHandlers.Add(triggerHandler.GetTriggerType(), triggerHandler);
            }
        }
    }
}