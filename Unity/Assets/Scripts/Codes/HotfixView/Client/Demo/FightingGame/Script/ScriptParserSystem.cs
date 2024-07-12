using System.Text.RegularExpressions;
using Sirenix.Utilities;

namespace ET.Client
{
    [FriendOf(typeof (ScriptParser))]
    [FriendOf(typeof (ScriptDispatcherComponent))]
    public static class ScriptParserSystem
    {
        public class ScriptParserAwakeSystem: AwakeSystem<ScriptParser, long>
        {
            protected override void Awake(ScriptParser self, long instanceID)
            {
                self.Cancel();
                self.instanceId = instanceID;
            }
        }

        public class ScriptParserLoadSystem: LoadSystem<ScriptParser>
        {
            protected override void Load(ScriptParser self)
            {
                self.Cancel();
            }
        }

        public class ScriptParserDestroySystem: DestroySystem<ScriptParser>
        {
            protected override void Destroy(ScriptParser self)
            {
                self.Cancel();
                self.instanceId = 0;
            }
        }

        public static void Cancel(this ScriptParser self)
        {
            self.subCoroutineDatas.Values.ForEach(data => { data.Recycle(); });
            self.subCoroutineDatas.Clear();
            self.Token?.Cancel();
            self.opLines = null;
            self.funcMap.Clear();
            self.markerMap.Clear();
            self.opDict.Clear();
        }

        public static void InitScript(this ScriptParser self, string opLine)
        {
            self.Cancel();

            //1. 建立语句和指针的映射
            self.opLines = opLine;
            string[] opLines = self.opLines.Split("\n");
            int pointer = 0;
            foreach (string opline in opLines)
            {
                string op = opline.Trim();
                if (string.IsNullOrEmpty(op) || op.StartsWith('#')) continue; //空行 or 注释行
                self.opDict[pointer++] = op;
            }

            //2. 匹配 func起始位置和marker
            foreach (var pair in self.opDict)
            {
                //函数指针
                string pattern = "@([^:]+)";
                Match match = Regex.Match(pair.Value, pattern);
                if (match.Success)
                {
                    self.funcMap.TryAdd(match.Groups[1].Value, pair.Key);
                }

                //匹配marker
                string pattern2 = @"SetMarker:\s+'([^']*)'";
                Match match2 = Regex.Match(pair.Value, pattern2);
                if (match2.Success)
                {
                    self.markerMap.TryAdd(match2.Groups[1].Value, pair.Key);
                }
            }

            self.Token = new ETCancellationToken();
        }

        public static int GetMarker(this ScriptParser self, string markerName)
        {
            if (self.markerMap.TryGetValue(markerName, out int index))
            {
                return index;
            }

            Log.Warning($"not found marker:{markerName}");
            return -1;
        }

        //Init ---> Init coroutine Main ---> Main Coroutine
        public static async ETTask<Status> Invoke(this ScriptParser self, string funcName)
        {
            await self.CallSubCoroutine(funcName, funcName);
            return Status.Success;
        }

        public static async ETTask<Status> CallSubCoroutine(this ScriptParser self, string funcName, string coroutineName)
        {
            //1. 函数入口指针
            if (!self.funcMap.TryGetValue(funcName, out int index))
            {
                Log.Warning($"not found function:{funcName}");
                return Status.Failed;
            }

            ETCancellationToken subToken = new();
            self.Token.Add(subToken.Cancel);
            SubCoroutineData coroutineData = SubCoroutineData.Create(coroutineName, index, subToken);
            //2. 回收同名协程
            if (self.subCoroutineDatas.TryGetValue(coroutineName, out SubCoroutineData _data))
            {
                _data.Recycle();
                self.subCoroutineDatas.Remove(coroutineName);
            }

            self.subCoroutineDatas.Add(coroutineName, coroutineData);

            while (++coroutineData.pointer < self.opDict.Count)
            {
                if (self.Token.IsCancel()) return Status.Failed;

                //4. 语句(OpType: xxxx;)根据OpType匹配handler
                self.ReplaceParam(self.opDict[coroutineData.pointer], out string opLine);
                Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                if (!match.Success)
                {
                    Log.Error($"{opLine}匹配失败!请检查格式");
                    return Status.Failed;
                }

                //5. 匹配 ScriptHandler
                string opType = match.Value;
                if (!ScriptDispatcherComponent.Instance.ScriptHandlers.TryGetValue(opType, out ScriptHandler handler))
                {
                    Log.Error($"not found script handler:{opType}");
                    return Status.Failed;
                }

                ScriptData scriptData = ScriptData.Create(opLine, coroutineName);
                Status ret = await handler.Handle(self, scriptData, subToken);
                scriptData.Recycle();

                if (subToken.IsCancel() || ret == Status.Failed) return Status.Failed;
                if (ret != Status.Success) return ret;
            }

            return Status.Success;
        }

        public static void StopSubCoroutine(this ScriptParser self, string CoroutineName)
        {
            if (!self.subCoroutineDatas.TryGetValue(CoroutineName, out SubCoroutineData coroutineData))
            {
                Log.Error($"not found coroutine:{CoroutineName}");
                return;
            }

            coroutineData.token.Cancel(); //取消子携程
            self.subCoroutineDatas.Remove(CoroutineName);
            coroutineData.Recycle();
        }

        public static void ReplaceParam(this ScriptParser self, string opLine, out string postOpLine)
        {
            postOpLine = opLine;

            //1.find <Param name = /> pattern string
            MatchCollection matches = Regex.Matches(opLine, @"<Param name = [^/]+/>");
            foreach (Match match in matches)
            {
                string paramLine = match.Value;

                //2. find param name
                Match match2 = Regex.Match(paramLine, "<Param name = (?<param>.*?)/>");
                if (!match2.Success)
                {
                    Log.Error($"dismatch {paramLine}");
                    return;
                }

                //3. replace
                string valueName = match2.Groups["param"].Value;
                Unit unit = Root.Instance.Get(self.instanceId) as Unit;
                postOpLine = postOpLine.Replace(paramLine, unit.GetComponent<TimelineComponent>().GetParameter(valueName).ToString());
            }
        }

        public static Unit GetUnit(this ScriptParser self)
        {
            Unit unit = Root.Instance.Get(self.instanceId) as Unit;
            return unit;
        }
    }
}