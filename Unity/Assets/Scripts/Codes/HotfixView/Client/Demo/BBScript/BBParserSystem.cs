using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    [FriendOf(typeof (ScriptDispatcherComponent))]
    public static class BBParserSystem
    {
        public class BBParserAwakeSystem : AwakeSystem<BBParser>
        {
            protected override void Awake(BBParser self)
            {
                EventSystem.Instance.Invoke(new ProcessBBScriptCallback(){ instanceId = self.InstanceId });
            }
        }

        public class BBParserLoadSystem : LoadSystem<BBParser>
        {
            protected override void Load(BBParser self)
            {
                EventSystem.Instance.Invoke(new ProcessBBScriptCallback(){ instanceId = self.InstanceId });
            }
        }
        
        public class BBParserDestroySystem: DestroySystem<BBParser>
        {
            protected override void Destroy(BBParser self)
            {
                self.Init();
            }
        }
        
        /// <summary>
        /// 取消当前所有子协程
        /// </summary>
        public static void Init(this BBParser self)
        {
            self.OpDict.Clear();
            //回收代码块
            foreach (var kv in self.GroupDict)
            {
                kv.Value.Recycle();
            }
            self.GroupDict.Clear();
            self.GroupPointerSet.Clear();
            //取消当前协程
            self.Cancel();
        }

        public static void Cancel(this BBParser self)
        {
            self.CancellationToken?.Cancel();
            self.Coroutine_Pointers.Clear();
            //回收共享变量
            foreach (var kv in self.ParamDict)
            {
                kv.Value.Recycle();
            }
            //销毁子组件
            self.ParamDict.Clear();
            foreach (Entity child in self.Children.Values)
            {
                child.Dispose();
            }
            self.CancellationToken = new ETCancellationToken();
        }
        
        /// <summary>
        /// 传入函数的头指针
        /// </summary>
        public static async ETTask<Status> Invoke(this BBParser self, int index, ETCancellationToken token)
        {
            //1. 热重载时销毁子协程
            self.CancellationToken.Add(token.Cancel);
            //2. 当前协程唯一标识符,生成协程ID和调用指针的映射关系
            long funcId = IdGenerater.Instance.GenerateInstanceId();
            self.Coroutine_Pointers.Add(funcId, index);

            //3. 逐条执行语句
            Status ret = Status.Success;
            while (++self.Coroutine_Pointers[funcId] < self.OpDict.Count)
            {
                // 语句(OPType: xxxx;) 根据 OPType 匹配handler
                string opLine = self.OpDict[self.Coroutine_Pointers[funcId]];
                // 因为用[GroupName]分割代码块，说明指针超出代码块了
                if (self.GroupPointerSet.Contains(self.Coroutine_Pointers[funcId]))
                {
                    ret = Status.Failed;
                    break;
                }
                // 匹配opType
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    Log.Error($"{opLine}匹配失败! 请检查格式");
                    ret = Status.Failed;
                    break;
                }
                string opType = match.Value;
                if (!ScriptDispatcherComponent.Instance.BBScriptHandlers.TryGetValue(opType, out BBScriptHandler handler))
                {
                    Log.Error($"not found script handler； {opType}");
                    ret = Status.Failed;
                    break;
                }

                // 执行当前指针的语句
                BBScriptData data = BBScriptData.Create(self.ReplaceParam(opLine), funcId); //池化，不然GC很高
                ret = await handler.Handle(self, data, token);
                data.Recycle();
                
                // 协程中断(热重载、unit被销毁了....)
                if (token.IsCancel() || ret != Status.Success)
                {
                    break;
                }
            }
            
            //4. 移除协程id
            self.Coroutine_Pointers.Remove(funcId);
            return ret;
        }
        
        /// <summary>
        ///  以子协程的形式执行代码块
        /// </summary>
        public static async ETTask<Status> RegistSubCoroutine(this BBParser self, int startIndex, int endIndex, ETCancellationToken token)
        {
            //1. 热重载时销毁子协程
            self.CancellationToken.Add(token.Cancel);
            //2. 生成协程Id
            long funcId = IdGenerater.Instance.GenerateInstanceId();
            self.Coroutine_Pointers.Add(funcId, startIndex);
            
            //3. 逐条执行语句
            Status ret = Status.Success;
            while (++self.Coroutine_Pointers[funcId] < endIndex)
            {
                //根据 opType 匹配 handler
                string opLine = self.OpDict[self.Coroutine_Pointers[funcId]];
                if (self.GroupPointerSet.Contains(self.Coroutine_Pointers[funcId]))
                {
                    ret = Status.Failed;
                    break;
                }
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    Log.Error($"{opLine}匹配失败! 请检查格式");
                    ret = Status.Failed;
                    break;
                }
                string opType = match.Value;
                if (!ScriptDispatcherComponent.Instance.BBScriptHandlers.TryGetValue(opType, out BBScriptHandler handler))
                {
                    Log.Error($"not found script handler: {opType}");
                    ret = Status.Failed;
                    break;
                }
                
                //执行语句
                BBScriptData data = BBScriptData.Create(self.ReplaceParam(opLine), funcId);
                ret = await handler.Handle(self, data, token);
                data.Recycle();
                
                if (token.IsCancel() || ret != Status.Success)
                {
                    break;
                }
            }
            
            //4. 移除协程id
            self.Coroutine_Pointers.Remove(funcId);
            return ret;
        }
        
        public static string ReplaceParam(this BBParser self, string opLine)
        {
            MatchCollection matches = Regex.Matches(opLine, @"\{(.*?)\}");
            string result = opLine;
            for (int i = 0; i < matches.Count; i++)
            {
                string content = matches[i].Groups[1].Value;
                string replace = EventSystem.Instance.Invoke<ReplaceParamCallback, string>(new ReplaceParamCallback()
                {
                    instanceId = self.InstanceId,
                    content = content, 
                });
                result = result.Replace(matches[i].Value, replace);
            }
            return result;
        }
        
        public static T RegistParam<T>(this BBParser self, string paramName, T value)
        {
            if (self.ParamDict.ContainsKey(paramName))
            {
                Log.Error($"already contain params:{paramName}");
                return default;
            }

            SharedVariable variable = SharedVariable.Create(paramName, value);
            self.ParamDict.Add(paramName, variable);
            return value;
        }

        public static void UpdateParam<T>(this BBParser self, string paramName, T value)
        {
            foreach ((string key, SharedVariable variable) in self.ParamDict)
            {
                if (!key.Equals(paramName))
                {
                    continue;
                }

                variable.value = value;
                return;
            }
            
            Log.Error($"does not exist param:{paramName}!");
        }
        
        public static T GetParam<T>(this BBParser self, string paramName)
        {
            if (!self.ParamDict.TryGetValue(paramName, out SharedVariable variable))
            {
                Log.Error($"does not exist param:{paramName}!");
                return default;
            }

            if (variable.value is not T value)
            {
                Log.Error($"cannot format {variable.name} to {typeof (T)}");
                return default;
            }

            return value;
        }

        public static bool ContainParam(this BBParser self, string paramName)
        {
            return self.ParamDict.ContainsKey(paramName);
        }
        
        public static void RemoveParam(this BBParser self, string paramName)
        {
            if (!self.ParamDict.ContainsKey(paramName))
            {
                Log.Error($"does not exist param:{paramName}!");
                return;
            }

            self.ParamDict[paramName].Recycle();
            self.ParamDict.Remove(paramName);
        }
        
        public static bool TryRemoveParam(this BBParser self, string paramName)
        {
            if (!self.ParamDict.ContainsKey(paramName))
            {
                return false;
            }
            
            self.ParamDict[paramName].Recycle();
            self.ParamDict.Remove(paramName);
            return true;
        }

        public static bool ContainGroup(this BBParser self, string groupName)
        {
            return self.GroupDict.ContainsKey(groupName);
        }

        public static int GetGroupPointer(this BBParser self, string groupName)
        {
            if (!self.GroupDict.TryGetValue(groupName, out DataGroup group))
            {
                Log.Error($"not found dataGroup: {groupName}");
                return -1;
            }
            return group.startIndex;
        }

        public static bool ContainFunction(this BBParser self, string groupName, string funcName)
        {
            if (!self.GroupDict.TryGetValue(groupName, out DataGroup group))
            {
                return false;
            }
            return group.funcPointers.ContainsKey(funcName);
        }
        
        public static int GetFunctionPointer(this BBParser self, string groupName, string funcName)
        {
            if (!self.GroupDict.TryGetValue(groupName, out DataGroup group))
            {
                Log.Error($"not found dataGroup: {groupName}");
                return -1;
            }
            if (!group.funcPointers.TryGetValue(funcName, out int funcPointer))
            {
                Log.Error($"not found funcPointer: {funcPointer}");
                return -1;
            }
            return funcPointer;
        }

        public static int GetMarkerPointer(this BBParser self, string groupName, string markerName)
        {
            if (!self.GroupDict.TryGetValue(groupName, out DataGroup group))
            {
                Log.Error($"not found dataGroup: {groupName}");
                return -1;
            }
            if (!group.markerPointers.TryGetValue(markerName, out int markerPointer))
            {
                Log.Error($"not found markerPointer: {markerPointer}");
                return -1;
            }
            return markerPointer;
        }
    }
}