using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
                ScriptHandler handler = Activator.CreateInstance(type) as ScriptHandler;
                if (handler == null)
                {
                    Log.Error($"this obj is not a ScriptableHandler: {type.Name}");
                    continue;
                }

                self.scriptHandlers.Add(handler.GetOPType(), handler);
            }
        }

        public static async ETTask<Status> Handle(this DialogueDispatcherComponent self, Unit unit, object node, ETCancellationToken token)
        {
            if (!self.dispatchHandlers.TryGetValue(node.GetType(), out NodeHandler handler))
            {
                Log.Error($"not found handler: {node}");
                return Status.Failed;
            }

            return await handler.Handle(unit, node, token);
        }

        public static int Check(this DialogueDispatcherComponent self, Unit unit, NodeCheckConfig nodeCheck)
        {
            if (!self.checker_dispatchHandlers.TryGetValue(nodeCheck.GetType(), out NodeCheckHandler nodeCheckerHandler))
            {
                throw new Exception($"not found nodeCheckerHandler: {nodeCheck}");
            }

            return nodeCheckerHandler.Check(unit, nodeCheck);
        }

        public static int Checks(this DialogueDispatcherComponent self, Unit unit, List<NodeCheckConfig> nodeCheckList)
        {
            foreach (var nodeCheck in nodeCheckList)
            {
                if (self.Check(unit, nodeCheck) != 0)
                {
                    return 1;
                }
            }

            return 0;
        }

        /// <summary>
        /// 一行指令 
        /// </summary>
        public static async ETTask ScriptHandle(this DialogueDispatcherComponent self, Unit unit, string opType, string opCode,
        ETCancellationToken token)
        {
            if (!self.scriptHandlers.TryGetValue(opType, out ScriptHandler handler))
            {
                Log.Error($"not found script handler: {opType}");
                return;
            }

            await handler.Handle(unit, opCode, token);
        }

        public static async ETTask ScriptHandles(this DialogueDispatcherComponent self, Unit unit, string scriptText, ETCancellationToken token)
        {
            //一行一行执行
            var opLines = scriptText.Split("\n");
            foreach (var opLine in opLines)
            {
                if (string.IsNullOrEmpty(opLine)) continue;
                if (token.IsCancel()) break;

                Match match = Regex.Match(opLine, @"^\w+");
                var opType = match.Value;
                var opCode = Regex.Match(opLine, "^(.*?);").Value; // ;后的不读取
                await self.ScriptHandle(unit, opType, opCode, token);
            }
        }
    }
}