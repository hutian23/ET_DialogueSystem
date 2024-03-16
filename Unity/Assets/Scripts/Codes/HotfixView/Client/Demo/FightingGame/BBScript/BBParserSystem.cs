using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    [FriendOf(typeof (DialogueDispatcherComponent))]
    [FriendOf(typeof (BehaviorBufferComponent))]
    [FriendOf(typeof (DialogueComponent))]
    public static class BBParserSystem
    {
        public class BBParserDestroySystem: DestroySystem<BBParser>
        {
            protected override void Destroy(BBParser self)
            {
                self.Cancel();
            }
        }

        /// <summary>
        /// 取消主协程以及其子协程
        /// </summary>
        public static void Cancel(this BBParser self)
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
            self.Cancel();
            self.opLines = node.BBScript;
            self.currentID = node.TargetID;
            //热重载取消所有BBParser子协程
            self.cancellationToken = new ETCancellationToken();
            self.GetParent<DialogueComponent>().token.Add(self.cancellationToken.Cancel);

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

        public static async ETTask Init(this BBParser self)
        {
            //行为信息组件
            BehaviorBufferComponent bufferComponent = self.GetParent<DialogueComponent>().GetComponent<BehaviorBufferComponent>();
            BehaviorInfo behaviorInfo = bufferComponent.AddChild<BehaviorInfo>();
            bufferComponent.behaviorDict.Add(self.currentID, behaviorInfo);

            await self.Invoke("Init");
        }

        public static async ETTask<Status> Main(this BBParser self)
        {
            Status ret = await self.Invoke("Main");
            self.cancellationToken.Cancel(); // 取消子协程
            return ret;
        }

        public static int GetMarker(this BBParser self, string markerName)
        {
            if (self.markers.TryGetValue(markerName, out int index))
            {
                return index;
            }

            Log.Error($"not found marker: {markerName}");
            return -1;
        }

        /// <summary>
        /// 同步调用 Main函数或者在Main函数中调用函数
        /// 异步调用 不需要记录指针
        /// </summary>
        public static async ETTask<Status> Invoke(this BBParser self, string funcName)
        {
            //1. 找到函数入口指针
            if (!self.funcMap.TryGetValue(funcName, out int index))
            {
                Log.Warning($"not found function : {funcName}");
                return Status.Failed;
            }

            //2. 当前协程唯一标识符,生成协程ID和调用指针的映射关系
            long funcId = IdGenerater.Instance.GenerateInstanceId(); 
            self.function_Pointers.Add(funcId, index);

            //3. 逐条执行语句
            while (++self.function_Pointers[funcId] < self.opDict.Count)
            {
                if (self.cancellationToken.IsCancel()) return Status.Failed;

                //4. 语句(A: xxxx;)根据A匹配handler
                string opLine = self.opDict[self.function_Pointers[funcId]];
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
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

                //5. 执行一条语句相当于一个子协程
                BBScriptData data = BBScriptData.Create(opLine, funcId, self.currentID); //池化，不然GC很高
                Status ret = await handler.Handle(self, data, self.cancellationToken); 
                data.Recycle();

                if (ret == Status.Return) return Status.Success;
                if (self.cancellationToken.IsCancel() || ret == Status.Failed) return Status.Failed;
            }

            return Status.Success;
        }
    }
}