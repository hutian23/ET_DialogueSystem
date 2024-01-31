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
        private static async ETTask ScriptHandle(this DialogueDispatcherComponent self, Unit unit, string opType, string opCode,
        ETCancellationToken token)
        {
            if (!self.scriptHandlers.TryGetValue(opType, out ScriptHandler handler))
            {
                Log.Error($"not found script handler: {opType}");
                return;
            }

            await handler.Handle(unit, opCode, token);
        }

        private static async ETTask CoroutineHandle(this DialogueDispatcherComponent self, Unit unit, List<string> corList, ETCancellationToken token)
        {
            int index = 0;
            while (index < corList.Count)
            {
                if (token.IsCancel())
                {
                    Log.Warning("canceld");
                    return;
                }

                var corLine = corList[index];
                if (string.IsNullOrEmpty(corLine) || corLine[0] == '#') // 空行 or 注释行 or 子命令
                {
                    index++;
                    continue;
                }

                var opLine = Regex.Split(corLine, @"- ")[1]; //把- 去掉，后面才是指令
                Match match = Regex.Match(opLine, @"^\w+");
                if (!match.Success)
                {
                    DialogueHelper.ScripMatchError(opLine);
                    return;
                }

                var opType = match.Value;
                var opCode = Regex.Match(opLine, "^(.*?);").Value; // ;后的不读取
                await self.ScriptHandle(unit, opType, opCode, token);
                if (token.IsCancel()) return;

                index++;
            }
        }

        public static async ETTask ScriptHandles(this DialogueDispatcherComponent self, Unit unit, string scriptText, ETCancellationToken token)
        {
            var opLines = scriptText.Split("\n"); // 一行一行执行
            int index = 0;

            while (index < opLines.Length)
            {
                var opLine = opLines[index];
                if (string.IsNullOrEmpty(opLine) || opLine[0] == '#' || opLine[0] == '-') // 空行 or 注释行 or 子命令
                {
                    index++;
                    continue;
                }

                if (opLine == "Coroutine:") // 携程行
                {
                    var corList = new List<string>();
                    while (++index < opLines.Length)
                    {
                        var coroutineLine = opLines[index];
                        if (string.IsNullOrEmpty(coroutineLine) || coroutineLine[0] == '#') continue;
                        if (coroutineLine[0] != '-') break;
                        corList.Add(coroutineLine);
                    }

                    self.CoroutineHandle(unit, corList, token).Coroutine();
                    continue;
                }

                Match match = Regex.Match(opLine, @"^\w+");
                if (!match.Success)
                {
                    DialogueHelper.ScripMatchError(opLine);
                    return;
                }

                var opType = match.Value;
                var opCode = Regex.Match(opLine, "^(.*?);").Value; // ;后的不读取

                await self.ScriptHandle(unit, opType, opCode, token);
                if (token.IsCancel()) return;

                index++;
            }
        }

        public static string GetReplaceStr(this DialogueDispatcherComponent self, Unit unit, string replaceType, string replaceText)
        {
            if (self.replaceHandlers.TryGetValue(replaceType, out ReplaceHandler handler))
            {
                return handler.GetReplaceStr(unit, replaceText);
            }

            return string.Empty;
        }
    }
}