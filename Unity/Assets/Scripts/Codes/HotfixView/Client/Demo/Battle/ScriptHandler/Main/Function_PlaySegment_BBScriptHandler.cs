using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    public class Function_PlaySegment_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Segment";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 匹配 trigger
            MatchCollection matches = Regex.Matches(data.opLine, @"\((.*?)\)");
            if (matches.Count == 0)
            {
                Log.Error($"Loop_Handler must have at least one triggerHandler!");
                return Status.Failed;
            }

            //2. 跳过PlaySegment片段
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndSegment:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = endIndex;
            
            //3. 启动Segment协程
            ETCancellationToken subToken = new();
            token.Add(subToken.Cancel);
            //3-1 检测协程
            CheckCoroutine(parser, startIndex, subToken).Coroutine();
            //3-2 Segment子协程
            await parser.RegistSubCoroutine(startIndex, endIndex, subToken);
            
            //4. 行为协程被取消了，退出
            return token.IsCancel() ? Status.Failed : Status.Success;
        }

        private async ETTask CheckCoroutine(BBParser self, int startIndex, ETCancellationToken token)
        {
            Unit unit = self.GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            
            while (true)
            {
                //1. 等待一帧再判断条件
                await bbTimer.WaitFrameAsync(token);
                //行为被取消了
                if (token.IsCancel()) return;
                
                string condition = self.OpDict[startIndex];
                bool result = true;
            
                //2. 匹配TriggerHandler
                MatchCollection matches = Regex.Matches(condition, @"\((.*?)\)");
                if (matches.Count == 0)
                {
                    Log.Error($"Segment_Handler must have at least one triggerHandler!");
                    result = false;
                }
            
                //3. 不符合条件，退出Segment子协程
                for (int i = 0; i < matches.Count; i++)
                {
                    string op = matches[i].Groups[1].Value;
                    Match triggerMatch = Regex.Match(op, @"(.*?):");
                
                    //匹配Trigger失败
                    if (!triggerMatch.Success)
                    {
                        ScriptHelper.ScripMatchError(op);
                        result = false;
                        break;
                    }
                
                    //执行检测
                    BBScriptData _data = BBScriptData.Create(op, 0);
                    bool ret = ScriptDispatcherComponent.Instance.GetTrigger(triggerMatch.Groups[1].Value).Check(self, _data);
                    if (!ret)
                    {
                        result = false;
                        break;
                    }
                }

                if (!result) break;
            }
            
            //4. 取消Segment协程
            token.Cancel();
        }
    }
}