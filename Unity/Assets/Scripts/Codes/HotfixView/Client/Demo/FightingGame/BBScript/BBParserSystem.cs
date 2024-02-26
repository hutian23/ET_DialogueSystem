using System.Text.RegularExpressions;

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

        public static void InitScript(this BBParser self, string ops)
        {
            //建立状态块和索引的映射
            self.funcMap.Clear();
            self.opLines = ops;

            var opLines = ops.Split("\n");
            for (int i = 0; i < opLines.Length; i++)
            {
                string opLine = opLines[i];
                if (string.IsNullOrEmpty(opLine)) continue;
                if (opLine[0] == '@')
                {
                    string pattern = "@([^:]+)";
                    Match match = Regex.Match(opLine, pattern);
                    if (!match.Success) continue;
                    self.funcMap.TryAdd(match.Groups[1].Value, i);
                }
            }
        }

        public static async ETTask Init(this BBParser self, ETCancellationToken token)
        { 
            await self.Invoke("Init", token);
        }
        
        private static async ETTask Invoke(this BBParser self, string funcName, ETCancellationToken token)
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
                if (token.IsCancel()) return;

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

                Status ret = await handler.Handle(unit, opCode, token);
                if (token.IsCancel() || ret == Status.Failed) return;
                await TimerComponent.Instance.WaitFrameAsync(token);
            }
        }
    }
}