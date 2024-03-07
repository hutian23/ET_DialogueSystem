using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    [FriendOf(typeof (DialogueDispatcherComponent))]
    public static class BBParserSystem
    {
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
            self.markers.Clear();
            self.function_Pointers.Clear();
        }

        public static void InitScript(this BBParser self, BBNode node)
        {
            self.Init();
            self.opLines = node.BBScript;
            self.currentID = node.TargetID;
            self.cancellationToken = new ETCancellationToken();
            self.function_Pointers.Clear();

            //建立函数和索引的映射
            var opLines = self.opLines.Split("\n");
            for (int i = 0; i < opLines.Length; i++)
            {
                string opLine = opLines[i];
                if (string.IsNullOrEmpty(opLine) || opLine[0] == '#') continue; //空行 or 注释行

                //匹配函数
                string pattern = "@([^:]+)";
                Match match = Regex.Match(opLine, pattern);
                if (match.Success)
                {
                    self.funcMap.TryAdd(match.Groups[1].Value, i);
                }

                //匹配标记
                string pattern2 = @"SetMarker:\s+'([^']*)'";
                Match match2 = Regex.Match(opLine, pattern2);
                if (match2.Success)
                {
                    self.markers.TryAdd(match2.Groups[1].Value, i);
                }
            }
        }

        public static async ETTask Init(this BBParser self, ETCancellationToken token)
        {
            //必杀配置，还有其他配置要作为子Entity挂在SkillInfo下面
            BBInputComponent inputComponent = self.GetParent<DialogueComponent>().GetComponent<BBInputComponent>();
            inputComponent.AddSkillInfo(self.currentID);
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

            //当前子协程的唯一标识符,对应调用索引
            long funcId = IdGenerater.Instance.GenerateInstanceId();
            self.function_Pointers.Add(funcId, index++);

            var opLines = self.opLines.Split("\n");
            while (self.function_Pointers[funcId] < opLines.Length)
            {
                if (token.IsCancel()) return Status.Failed;
                
                string opLine = opLines[self.function_Pointers[funcId]++];
                //空行 or 注释行，跳过
                if (string.IsNullOrEmpty(opLine) || opLine[0] == '#') continue;
                
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    Log.Error($"{opLine}匹配失败! 请检查格式");
                    return Status.Failed;
                }
                
                //匹配handler
                string opType = match.Value;
                string opCode = Regex.Match(opLine, "^(.*?);").Value;
                if (!DialogueDispatcherComponent.Instance.BBScriptHandlers.TryGetValue(opType, out BBScriptHandler handler))
                {
                    Log.Error($"not found script handler； {opType}");
                    return Status.Failed;
                }
                
                //执行一个指令相当于一个子协程
                BBScriptData data = BBScriptData.Create(opCode, funcId);
                Status ret = await handler.Handle(self, data, token);
                data.Recycle();
                if (ret == Status.Return) return Status.Success;
                if (token.IsCancel() || ret == Status.Failed) return Status.Failed;
                await TimerComponent.Instance.WaitFrameAsync(token);
            }

            return Status.Success;
        }
    }
}