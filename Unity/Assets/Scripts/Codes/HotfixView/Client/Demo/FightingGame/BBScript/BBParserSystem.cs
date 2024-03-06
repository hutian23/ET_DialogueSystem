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
                self.Init();
            }
        }

        public static void Init(this BBParser self)
        {
            self.cancellationToken?.Cancel();
            self.funcMap.Clear();
            self.opLines = null;
        }

        public static void InitScript(this BBParser self, BBNode node)
        {
            self.Init();
            self.opLines = node.BBScript;
            self.currentID = node.TargetID;
            self.cancellationToken = new ETCancellationToken();

            //建立状态块和索引的映射
            var opLines = self.opLines.Split("\n");
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

        public static async ETTask Init(this BBParser self, ETCancellationToken token)
        {
            //必杀配置，还有其他配置要作为子Entity挂在SkillInfo下面
            BBInputComponent inputComponent = self.GetParent<DialogueComponent>().GetComponent<BBInputComponent>();
            inputComponent.RemoveChild(self.currentID);
            inputComponent.AddChild<BBSkillInfo,uint>(self.currentID);
            
            await self.Invoke("Init", token);
        }

        public static async ETTask<Status> Main(this BBParser self, ETCancellationToken token)
        {
            Status ret = await self.Invoke("Main", token);
            self.cancellationToken.Cancel(); //Main执行完毕，取消所有子协程
            return ret;
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

                string opLine = opLines[index++];
                //空行 or 注释行，跳过
                if (string.IsNullOrEmpty(opLine) || opLine[0] == '#') continue;

                Unit unit = self.GetParent<DialogueComponent>().GetParent<Unit>();
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    Log.Error($"{opLine}匹配失败! 请检查格式");
                    return Status.Failed;
                }

                var opType = match.Value;
                var opCode = Regex.Match(opLine, "^(.*?);").Value;
                if (!DialogueDispatcherComponent.Instance.BBScriptHandlers.TryGetValue(opType, out BBScriptHandler handler))
                {
                    Log.Error($"not found script handler； {opType}");
                    return Status.Failed;
                }

                Status ret = await handler.Handle(unit, opCode, token);
                if (token.IsCancel() || ret == Status.Failed) return Status.Failed;
                //对应return
                if (ret != Status.Success) return Status.Success;
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

                string opLine = opLines[index++];
                //空行 or 注释行，跳过
                if (string.IsNullOrEmpty(opLine) || opLine[0] == '#') continue;

                Unit unit = self.GetParent<DialogueComponent>().GetParent<Unit>();
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    Log.Error($"{opLine}匹配失败! 请检查格式");
                    return;
                }

                var opType = match.Value;
                var opCode = Regex.Match(opLine, "^(.*?);").Value;
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