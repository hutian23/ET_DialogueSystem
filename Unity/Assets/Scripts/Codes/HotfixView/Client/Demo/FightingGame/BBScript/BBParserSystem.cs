﻿using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    [FriendOf(typeof (DialogueDispatcherComponent))]
    public static class BBParserSystem
    {
        public class BBParserLoadSystem: LoadSystem<BBParser>
        {
            protected override void Load(BBParser self)
            {
                self.Dispose();
            }
        }

        public class BBParserDestroySystem: DestroySystem<BBParser>
        {
            protected override void Destroy(BBParser self)
            {
                self.BBParser_Destroy();
            }
        }

        private static void BBParser_Destroy(this BBParser self)
        {
            self.cancellationToken?.Cancel();
            self.funcMap.Clear();
            self.opLines = null;
        }

        public static void InitScript(this BBParser self, string ops)
        {
            self.BBParser_Destroy();
            self.opLines = ops;
            self.cancellationToken = new ETCancellationToken();

            //建立状态块和索引的映射
            var opLines = ops.Split("\n");
            for (int i = 0; i < opLines.Length; i++)
            {
                string opLine = opLines[i];
                if (string.IsNullOrEmpty(opLine) || opLine[0] != '@') continue;
                string pattern = "@([^:]+)";
                Match match = Regex.Match(opLine, pattern);
                if (!match.Success) continue;
                self.funcMap.TryAdd(match.Groups[1].Value, i);
            }
        }

        public static int GetFunctionPointer(this BBParser self, string functionName)
        {
            if (!self.funcMap.TryGetValue(functionName, out int pointer))
            {
                Log.Error($"not found function : {functionName}");
                return 0;
            }

            return pointer;
        }

        public static async ETTask Init(this BBParser self, ETCancellationToken token)
        {
            await self.Invoke("Init", token);
        }

        public static async ETTask Main(this BBParser self, ETCancellationToken token)
        {
            await self.Invoke("State_Main", token);
        }

        /// <summary>
        /// 同步调用 Main函数或者在Main函数中调用函数
        /// 异步调用 不需要记录指针
        /// </summary>
        public static async ETTask<Status> Invoke(this BBParser self, string funcName, ETCancellationToken token)
        {
            if (!self.funcMap.TryGetValue(funcName, out int index))
            {
                Log.Warning($"not found function : {funcName}");
                return Status.Failed;
            }

            index++;

            var opLines = self.opLines.Split("\n");

            while (index < opLines.Length)
            {
                if (token.IsCancel()) return Status.Failed;

                string opLine = opLines[index];
                //空行 or 注释行，跳过
                if (string.IsNullOrEmpty(opLine) || opLine[0] == '#')
                {
                    index++;
                    continue;
                }

                Unit unit = self.GetParent<DialogueComponent>().GetParent<Unit>();
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    Log.Error($"{opLine}匹配失败! 请检查格式");
                    return Status.Failed;
                }

                var opType = match.Value;
                var opCode = Regex.Match(opLine, "^(.*?);").Value;

                if (opType.Equals("return")) return Status.Failed;
                if (!DialogueDispatcherComponent.Instance.BBScriptHandlers.TryGetValue(opType, out BBScriptHandler handler))
                {
                    Log.Error($"not found script handler； {opType}");
                    return Status.Failed;
                }

                Status ret = await handler.Handle(unit, opCode, token);
                if (token.IsCancel() || ret == Status.Failed) return Status.Failed;
                await TimerComponent.Instance.WaitFrameAsync(token);
            }

            return Status.Success;
        }

        public static async ETTask SubCoroutine(this BBParser self, string funcName)
        {
            if (!self.funcMap.TryGetValue(funcName, out int index))
            {
                Log.Warning($"not found function : {funcName}");
                return;
            }

            index++;

            var opLines = self.opLines.Split("\n");

            while (index < opLines.Length)
            {
                if (self.cancellationToken.IsCancel()) return;

                string opLine = opLines[index];
                //空行 or 注释行，跳过
                if (string.IsNullOrEmpty(opLine) || opLine[0] == '#')
                {
                    index++;
                    continue;
                }

                Unit unit = self.GetParent<DialogueComponent>().GetParent<Unit>();
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    Log.Error($"{opLine}匹配失败! 请检查格式");
                    return;
                }

                var opType = match.Value;
                var opCode = Regex.Match(opLine, "^(.*?);").Value;

                if (opType.Equals("return")) return;
                if (!DialogueDispatcherComponent.Instance.BBScriptHandlers.TryGetValue(opType, out BBScriptHandler handler))
                {
                    Log.Error($"not found script handler； {opType}");
                    return;
                }

                Status ret = await handler.Handle(unit, opCode, self.cancellationToken);
                if (self.cancellationToken.IsCancel() || ret == Status.Failed) return;
                await TimerComponent.Instance.WaitFrameAsync(self.cancellationToken);
            }
        }
    }
}