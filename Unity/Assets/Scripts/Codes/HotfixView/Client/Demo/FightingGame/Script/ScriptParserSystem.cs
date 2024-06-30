using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (ScriptParser))]
    public static class ScriptParserSystem
    {
        public class ScriptParserAwakeSystem: AwakeSystem<ScriptParser>
        {
            protected override void Awake(ScriptParser self)
            {
            }
        }

        private static void Cancel(this ScriptParser self)
        {
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

        public static async ETTask<Status> Invoke(this ScriptParser self, string funcName, ETCancellationToken token)
        {
            //1. 函数入口指针
            if (!self.funcMap.TryGetValue(funcName, out int index))
            {
                Log.Warning($"not found function:{funcName}");
                return Status.Failed;
            }
            Log.Warning(index.ToString());

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}