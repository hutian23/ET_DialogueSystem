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
            self.opDict.Clear();
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

            //建立执行语句和指针的映射
            string[] opLines = self.opLines.Split("\n");
            int pointer = 0;
            foreach (string opLine in opLines)
            {
                string op = opLine.Trim();
                if (string.IsNullOrEmpty(op) || op.StartsWith('#')) continue; //空行 or 注释行
                self.opDict[pointer++] = op;
            }

            foreach (var kv in self.opDict)
            {
                //函数指针
                string pattern = "@([^:]+)";
                Match match = Regex.Match(kv.Value, pattern);
                if (match.Success)
                {
                    self.funcMap.TryAdd(match.Groups[1].Value, kv.Key);
                }

                //匹配marker
                string pattern2 = @"SetMarker:\s+'([^']*)'";
                Match match2 = Regex.Match(kv.Value, pattern2);
                if (match2.Success)
                {
                    self.markers.TryAdd(match2.Groups[1].Value, kv.Key);
                }
            }
        }

        public static async ETTask Init(this BBParser self, ETCancellationToken token)
        {
            //技能配置，还有其他配置要作为子Entity挂在SkillInfo下面
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

        public static int GetMarker(this BBParser self, string markerName)
        {
            if (!self.markers.TryGetValue(markerName, out int index))
            {
                Log.Error($"not found marker: {markerName}");
                return -1;
            }

            return index;
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

            long funcId = IdGenerater.Instance.GenerateInstanceId(); //当前子协程的唯一标识符,对应调用指针
            self.function_Pointers.Add(funcId, index);
            
            while (++self.function_Pointers[funcId] < self.opDict.Count)
            {
                if (token.IsCancel()) return Status.Failed;

                string opLine = self.opDict[self.function_Pointers[funcId]];
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?"); //匹配handler
                if (!match.Success)
                {
                    Log.Error($"{opLine}匹配失败! 请检查格式");
                    return Status.Failed;
                }
                string opType = match.Value;
                if (opType == "SetMarker") continue; //Init时执行过，跳过

                if (!DialogueDispatcherComponent.Instance.BBScriptHandlers.TryGetValue(opType, out BBScriptHandler handler))
                {
                    Log.Error($"not found script handler； {opType}");
                    return Status.Failed;
                }

                BBScriptData data = BBScriptData.Create(opLine, funcId); //池化，不然GC很高
                Status ret = await handler.Handle(self, data, token); //执行一条语句相当于一个子协程
                data.Recycle();

                if (ret == Status.Return) return Status.Success;
                if (token.IsCancel() || ret == Status.Failed) return Status.Failed;
            }

            return Status.Success;
        }
    }
}