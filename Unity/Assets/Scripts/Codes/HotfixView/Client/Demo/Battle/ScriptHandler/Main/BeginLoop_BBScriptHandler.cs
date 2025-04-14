using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.LoopTimer)]
    [FriendOf(typeof(BBParser))]
    public class LoopTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            //1, match trigger
            int startIndex = self.GetParam<int>("BeginLoop_TriggerIndex");
            string LoopTrigger = self.OpDict[startIndex];
            bool result = true;
            
            MatchCollection matches = Regex.Matches(LoopTrigger, @"\((.*?)\)");
            if (matches.Count == 0)
            {
                Log.Error($"Loop_Handler must have at least one triggerHandler!");
                result = false;
            }

            //2. exec trigger handler
            for (int i = 0; i < matches.Count; i++)
            {
                string op = matches[i].Groups[1].Value;
                Match triggerMatch = Regex.Match(op, @"(.*?):");
                //match failed
                if (!triggerMatch.Success)
                {
                    ScriptHelper.ScripMatchError(op);
                    result = false;
                    break; 
                }
                
                BBScriptData _data = BBScriptData.Create(self.ReplaceParam(op), 0);
                bool ret = ScriptDispatcherComponent.Instance.GetTrigger(triggerMatch.Groups[1].Value).Check(self, _data);
                if (!ret)
                {
                    result = false;
                    break;
                }   
            }
            
            if (result) return;
               
            //EndLoop
            //销毁定时器
            BBTimerComponent bbTimer = self.GetParent<Unit>().GetComponent<BBTimerComponent>();
            long loopTimer = self.GetParam<long>("BeginLoop_Timer");
            bbTimer.Remove(ref loopTimer);
            //取消loop协程
            ETCancellationToken loopToken = self.GetParam<ETCancellationToken>("BeginLoop_Token");
            loopToken.Cancel();

            self.TryRemoveParam("BeginLoop_Timer");
            self.TryRemoveParam("BeginLoop_TriggerIndex");
            self.TryRemoveParam("BeginLoop_Token");
        }
    }
    
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(ScriptDispatcherComponent))]
    public class BeginLoop_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BeginLoop";
        }

        //只能在Main携程中使用
        //BeginLoop: (None), (inAir: true), (TransitionCached: 'RunToIdle')
        //EndLoop
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            MatchCollection matches = Regex.Matches(data.opLine, @"\((.*?)\)");
            if (matches.Count == 0)
            {
                Log.Error($"Loop_Handler must have at least one triggerHandler!");
                return Status.Failed;
            }
            
            BBTimerComponent bbTimer = parser.GetParent<Unit>().GetComponent<BBTimerComponent>();
            
            //跳过BeginLoop代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndLoop:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = endIndex;

            //不是执行完代码块之后再判定是否符合条件进入下一次循环，如果条件不符合直接退出Loop协程
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.LoopTimer, parser);
            ETCancellationToken loopToken = new();
            parser.RegistParam("BeginLoop_Timer", timer); 
            parser.RegistParam("BeginLoop_Token", loopToken);
            parser.RegistParam("BeginLoop_TriggerIndex", startIndex);
            
            token.Add(() =>
            {
                bbTimer.Remove(ref timer);
                loopToken.Cancel();
            });
            
            //4. start loop coroutine
            await LoopCoroutine(parser, startIndex, endIndex, loopToken);
            
            return token.IsCancel()? Status.Failed : Status.Success;
        }

        private async ETTask<Status> LoopCoroutine(BBParser self, int startIndex, int endIndex, ETCancellationToken token)
        {
            while (true)
            {
                Status ret = await self.RegistSubCoroutine(startIndex, endIndex, token);
                if (ret != Status.Success || token.IsCancel()) return ret;
                
                //避免卡死
                await TimerComponent.Instance.WaitFrameAsync(token);
                if (token.IsCancel()) return Status.Failed;
            }
        }
    }
}