using System;

namespace ET.Client
{
    [FriendOf(typeof (ScriptDispatcherComponent))]
    public static class ScriptDispatcherComponentSystem
    {
        public class ScriptDispatcherComponentAwakeSystem: AwakeSystem<ScriptDispatcherComponent>
        {
            protected override void Awake(ScriptDispatcherComponent self)
            {
                ScriptDispatcherComponent.Instance = self;
                self.Init();
            }
        }

        public class ScriptDispatcherComponentLoadSystem: LoadSystem<ScriptDispatcherComponent>
        {
            protected override void Load(ScriptDispatcherComponent self)
            {
                self.Init();
            }
        }

        public class ScriptDispatcherComponentDestroySystem: DestroySystem<ScriptDispatcherComponent>
        {
            protected override void Destroy(ScriptDispatcherComponent self)
            {
                self.BBScriptHandlers.Clear();
                self.BBTriggerHandlers.Clear();
                self.InputHandlers.Clear();
                self.BBValueDict.Clear();
                ScriptDispatcherComponent.Instance = null;
            }
        }

        private static void Init(this ScriptDispatcherComponent self)
        {
            self.BBScriptHandlers.Clear();
            var bbScriptHandlers = EventSystem.Instance.GetTypes(typeof (BBScriptAttribute));
            foreach (var bbScript in bbScriptHandlers)
            {
                BBScriptHandler handler = Activator.CreateInstance(bbScript) as BBScriptHandler;
                if (handler == null)
                {
                    Log.Error($"this obj is not a bbScriptHandler:{bbScript.Name} ");
                    continue;
                }

                self.BBScriptHandlers.Add(handler.GetOPType(), handler);
            }

            self.BBTriggerHandlers.Clear();
            var bbTriggerHandlers = EventSystem.Instance.GetTypes(typeof (BBTriggerAttribute));
            foreach (var bbTrigger in bbTriggerHandlers)
            {
                BBTriggerHandler handler = Activator.CreateInstance(bbTrigger) as BBTriggerHandler;
                if (handler == null)
                {
                    Log.Error($"this obj is not a bbTriggerHandler:{bbTrigger.Name}");
                    continue;
                }
                
                self.BBTriggerHandlers.Add(handler.GetTriggerType(), handler);
            }
            
            self.InputHandlers.Clear();
            var inputHandlers = EventSystem.Instance.GetTypes(typeof (InputAttribute));
            foreach (var inputHandler in inputHandlers)
            {
                InputHandler handler = Activator.CreateInstance(inputHandler) as InputHandler;
                if (handler == null)
                {
                    Log.Error($"this obj is not a inputHandler: {inputHandler.Name}");
                    continue;
                }

                self.InputHandlers.Add(handler.GetHandlerType(), handler);
            } 
        }

        public static BBScriptHandler GetScriptHandler(this ScriptDispatcherComponent self, string name)
        {
            if (!self.BBScriptHandlers.TryGetValue(name, out BBScriptHandler handler))
            {
                Log.Error($"not found scriptHandler: {name}");
                return null;
            }
            return handler;
        }
        
        public static BBTriggerHandler GetTrigger(this ScriptDispatcherComponent self, string name)
        {
            if (!self.BBTriggerHandlers.TryGetValue(name, out BBTriggerHandler handler))
            {
                Log.Error($"not found triggerHandler: {name}");
                return null;
            }
            return handler;
        }

        public static InputHandler GetInputHandler(this ScriptDispatcherComponent self, string name)
        {
            if (!self.InputHandlers.TryGetValue(name, out InputHandler handler))
            {
                Log.Error($"not found inputHandler: {name}");
                return null;
            }

            return handler;
        }
    }
}