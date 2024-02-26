using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BBParser))]
    public static class BBParserSystem
    {
        public class BBParserLoadSystem: LoadSystem<BBParser>
        {
            protected override void Load(BBParser self)
            {
                self.Dispose();
            }
        }

        public static void Init(this BBParser self, string ops)
        {
            //建立状态块和索引的映射
            self.funcMap.Clear();
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

        public static void CallFunc(this BBParser self, string ops)
        {
            
        }
                
    }
}