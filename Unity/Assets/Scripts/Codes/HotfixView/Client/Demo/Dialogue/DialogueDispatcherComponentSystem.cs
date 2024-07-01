using System;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof (DialogueDispatcherComponent))]
    public static class DialogueDispatcherComponentSystem
    {
        public class DialogueDispatcherComponentAwakeSystem: AwakeSystem<DialogueDispatcherComponent>
        {
            protected override void Awake(DialogueDispatcherComponent self)
            {
                DialogueDispatcherComponent.Instance = self;
                self.Init();
            }
        }

        public class DialogueDispatcherComponentLoadSystem: LoadSystem<DialogueDispatcherComponent>
        {
            protected override void Load(DialogueDispatcherComponent self)
            {
                self.Init();
            }
        }

        public class DialogueDispatcherComponentDestorySystem: DestroySystem<DialogueDispatcherComponent>
        {
            protected override void Destroy(DialogueDispatcherComponent self)
            {
                self.dispatchHandlers.Clear();
                DialogueDispatcherComponent.Instance = null;
            }
        }

        private static void Init(this DialogueDispatcherComponent self)
        {
            self.dispatchHandlers.Clear();
            var nodeHandlers = EventSystem.Instance.GetTypes(typeof (DialogueAttribute));
            foreach (Type type in nodeHandlers)
            {
                NodeHandler nodeHandler = Activator.CreateInstance(type) as NodeHandler;
                if (nodeHandler == null)
                {
                    Log.Error($"this nodeHandler is not nodeHandler!: {type.Name}");
                    continue;
                }

                self.dispatchHandlers.Add(nodeHandler.GetDialogueType(), nodeHandler);
            }

            self.checker_dispatchHandlers.Clear();
            var nodeCheckerHandlers = EventSystem.Instance.GetTypes(typeof (NodeCheckerAttribute));
            foreach (Type type in nodeCheckerHandlers)
            {
                NodeCheckHandler nodeCheckHandler = Activator.CreateInstance(type) as NodeCheckHandler;
                if (nodeCheckHandler == null)
                {
                    Log.Error($"this obj is not a nodeCheckerHandler:{type.Name}");
                    continue;
                }

                self.checker_dispatchHandlers.Add(nodeCheckHandler.GetNodeCheckType(), nodeCheckHandler);
            }

            self.scriptHandlers.Clear();
            var scriptHandlers = EventSystem.Instance.GetTypes(typeof (DialogueScriptAttribute));
            foreach (Type type in scriptHandlers)
            {
                DialogueScriptHandler handler = Activator.CreateInstance(type) as DialogueScriptHandler;
                if (handler == null)
                {
                    Log.Error($"this obj is not a ScriptableHandler: {type.Name}");
                    continue;
                }

                self.scriptHandlers.Add(handler.GetOPType(), handler);
            }

            self.replaceHandlers.Clear();
            var replaceHandlers = EventSystem.Instance.GetTypes(typeof (DialogueReplaceAttribute));
            foreach (var type in replaceHandlers)
            {
                ReplaceHandler handler = Activator.CreateInstance(type) as ReplaceHandler;
                if (handler == null)
                {
                    Log.Error($"this obj is not a replaceHandler: {type.Name}");
                    continue;
                }

                self.replaceHandlers.Add(handler.GetReplaceType(), handler);
            }

            self.BBCheckHandlers.Clear();
            var bbHandlers = EventSystem.Instance.GetTypes(typeof (BBScriptCheckAttribute));
            foreach (var checker in bbHandlers)
            {
                BBCheckHandler handler = Activator.CreateInstance(checker) as BBCheckHandler;
                if (handler == null)
                {
                    Log.Error($"this obj is not a BBCheckerHandler: {checker.Name}");
                    continue;
                }

                self.BBCheckHandlers.Add(handler.GetBehaviorType(), handler);
            }

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
            foreach (var bbtrigger in bbTriggerHandlers)
            {
                BBTriggerHandler handler = Activator.CreateInstance(bbtrigger) as BBTriggerHandler;
                if (handler == null)
                {
                    Log.Error($"this obj is not a bbTriggerHandler:{bbtrigger.Name}");
                    continue;
                }
                
                self.BBTriggerHandlers.Add(handler.GetTriggerType(), handler);
            }
        }

        public static async ETTask<Status> Handle(this DialogueDispatcherComponent self, Unit unit, object node, ETCancellationToken token)
        {
            if (self.dispatchHandlers.TryGetValue(node.GetType(), out NodeHandler handler))
            {
                //执行脚本
                await DialogueDispatcherComponent.Instance.ScriptHandles(unit, node as DialogueNode, token);
                if (token.IsCancel())
                {
                    return Status.Failed;
                }
                return await handler.Handle(unit, node, token);
            }

            Log.Error($"not found handler: {node}");
            return Status.Failed;
        }

        private static int Check(this DialogueDispatcherComponent self, Unit unit, NodeCheckConfig nodeCheck)
        {
            if (!self.checker_dispatchHandlers.TryGetValue(nodeCheck.GetType(), out NodeCheckHandler nodeCheckerHandler))
            {
                throw new Exception($"not found nodeCheckerHandler: {nodeCheck}");
            }

            return nodeCheckerHandler.Check(unit, nodeCheck);
        }

        public static int Checks(this DialogueDispatcherComponent self, Unit unit, List<NodeCheckConfig> nodeCheckList)
        {
            if (nodeCheckList == null) return 0;
            foreach (var nodeCheck in nodeCheckList)
            {
                if (self.Check(unit, nodeCheck) != 0)
                {
                    return 1;
                }
            }

            return 0;
        }

        public static string GetReplaceStr(this DialogueDispatcherComponent self, Unit unit, string replaceType, string replaceText)
        {
            if (self.replaceHandlers.TryGetValue(replaceType, out ReplaceHandler handler))
            {
                return handler.GetReplaceStr(unit, replaceText);
            }

            return string.Empty;
        }

        public static BBCheckHandler GetBBCheckHandler(this DialogueDispatcherComponent self, string name)
        {
            if (self.BBCheckHandlers.TryGetValue(name, out BBCheckHandler handler))
            {
                return handler;
            }

            return null;
        }

        public static BBTriggerHandler GetTrigger(this DialogueDispatcherComponent self, string name)
        {
            if (!self.BBTriggerHandlers.TryGetValue(name, out BBTriggerHandler handler))
            {
                Log.Error($"not found triggerHandler: {name}");
                return null;
            }

            return handler;
        }
    }
}