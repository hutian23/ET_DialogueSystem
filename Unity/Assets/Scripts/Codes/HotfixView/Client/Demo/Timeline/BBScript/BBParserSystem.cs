using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    [FriendOf(typeof (DialogueDispatcherComponent))]
    [FriendOf(typeof (BehaviorBufferComponent))]
    [FriendOf(typeof (DialogueComponent))]
    [FriendOf(typeof (BehaviorInfo))]
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

            //回收变量
            foreach (var kv in self.paramDict)
            {
                kv.Value.Recycle();
            }

            self.paramDict.Clear();
        }

        public static void InitScript(this BBParser self, string script)
        {
            self.Cancel();
            self.opLines = script;

            //热重载取消所有BBParser子协程
            self.cancellationToken = new ETCancellationToken();

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

        public static async ETTask<Status> Main(this BBParser self)
        {
            Status ret = await self.Invoke("Main", self.cancellationToken);
            self.Exit();
            // 取消子协程
            self.cancellationToken?.Cancel();
            return ret;
        }

        public static void Init(this BBParser self)
        {
            BBPlayableGraph bbPlayable = self.GetParent<TimelineComponent>().GetTimelinePlayer().BBPlayable;
            foreach (BBTimeline timeline in bbPlayable.GetTimelines())
            {
                //运行初始化协程
                self.InitScript(timeline.Script);
                self.RegistParam("BehaviorOrder", timeline.order);
                self.Invoke("Init", self.cancellationToken).Coroutine();
            }
        }

        /// <summary>
        /// 退出当前行为
        /// </summary>
        /// <param name="self"></param>
        private static void Exit(this BBParser self)
        {
            async ETTask ExitCoroutine()
            {
                ETCancellationToken exitToken = new();
                await self.Invoke("Exit", exitToken);
                exitToken.Cancel();
            }

            ExitCoroutine().Coroutine();

            //如果当前行为是正常执行完毕(没有被取消)，则回到默认状态
            //TODO 这导致和SkillBufferComponent耦合，希望想到办法优化这里
            if (!self.cancellationToken.IsCancel())
            {
                EventSystem.Instance.Invoke(new CancelBehaviorCallback() { instanceId = self.InstanceId });
            }
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
        public static async ETTask<Status> Invoke(this BBParser self, string funcName, ETCancellationToken token)
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
                if (token.IsCancel()) return Status.Failed;

                //4. 语句(OPType: xxxx;) 根据 OPType 匹配handler
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
                Status ret = await handler.Handle(self, data, token);
                data.Recycle();

                if (token.IsCancel() || ret == Status.Failed) return Status.Failed;
                if (ret != Status.Success) return ret;
            }

            return Status.Success;
        }

        /// <summary>
        /// 虽然不知道有什么用但还是做一下
        /// 加特林取消窗口输入检测实际上是一个子协程，我想要注册到FunctionMap中
        /// 主协程执行完毕才能取消子协程太麻烦了，我想支持随时可以取消子协程
        /// </summary>
        public static ETCancellationToken RegistSubCoroutine(this BBParser self, string funcName)
        {
            if (self.subCoroutineDict.ContainsKey(funcName))
            {
                Log.Error($"already contain subCoroutine: {funcName}!!!");
                return null;
            }

            ETCancellationToken subToken = new();
            self.cancellationToken.Add(subToken.Cancel);
            self.subCoroutineDict.Add(funcName, subToken);
            return subToken;
        }

        public static void CancelSubCoroutine(this BBParser self, string funcName)
        {
            if (!self.subCoroutineDict.TryGetValue(funcName, out ETCancellationToken subToken))
            {
                return;
            }

            self.cancellationToken?.Remove(subToken.Cancel);
            subToken.Cancel();
        }

        public static T RegistParam<T>(this BBParser self, string paramName, T value)
        {
            if (self.paramDict.ContainsKey(paramName))
            {
                Log.Error($"already contain params:{paramName}");
                return default;
            }

            SharedVariable variable = SharedVariable.Create(paramName, value);
            self.paramDict.Add(paramName, variable);
            return value;
        }

        public static T GetParam<T>(this BBParser self, string paramName)
        {
            if (!self.paramDict.ContainsKey(paramName))
            {
                Log.Error($"does not exist param:{paramName}!");
                return default;
            }

            SharedVariable variable = self.paramDict[paramName];
            if (variable.value is not T value)
            {
                Log.Error($"cannot format {variable.name} to {typeof (T)}");
                return default;
            }

            return value;
        }

        public static void RemoveParam(this BBParser self, string paramName)
        {
            if (!self.paramDict.ContainsKey(paramName))
            {
                Log.Error($"does not exist param:{paramName}!");
                return;
            }

            self.paramDict[paramName].Recycle();
            self.paramDict.Remove(paramName);
        }
    }
}