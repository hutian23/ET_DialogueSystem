using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(LoopComponent))]
    public static class LoopComponentSystem
    {
        public class LoopComponentDestroySystem : DestroySystem<LoopComponent>
        {
            protected override void Destroy(LoopComponent self)
            {
                self.triggerIndex = 0;
                self.startIndex = 0;
                self.curIndex = 0;
                self.endIndex = 0;
                self.token.Cancel();
            }
        }

        public static async ETTask TriggerCor(this LoopComponent self)
        {
            BBParser parser = self.GetParent<BBParser>();
            // BBTimerComponent bbTimer = parser.GetParent<Unit>().GetComponent<BBTimerComponent>();
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();    
            
            while (true)
            {
                // await bbTimer.WaitFrameAsync(self.token);
                await postStepTimer.WaitFrameAsync(self.token);
                if (self.token.IsCancel()) break;
                
                //1. Match trigger
                string loopTrigger = parser.OpDict[self.triggerIndex];
                MatchCollection matches = Regex.Matches(loopTrigger,@"\((.*?)\)");
                if (matches.Count == 0)
                {
                    Log.Error($"Loop_Handler must have at least one triggerHandler!");
                    break;
                }
                
                //2. Run TriggerHandler
                for (int i = 0; i < matches.Count; i++)
                {
                    string op = matches[i].Groups[1].Value;
                    Match triggerMatch = Regex.Match(op, @"(.*?):");
                    
                    // Match Failed
                    if (!triggerMatch.Success)
                    {
                        ScriptHelper.ScripMatchError(op);
                        break;
                    }
                    
                    // Match Success
                    BBScriptData _data = BBScriptData.Create(op, 0);
                    bool ret = ScriptDispatcherComponent.Instance.GetTrigger(triggerMatch.Groups[1].Value).Check(parser, _data);
                    if (ret) continue;
                    
                    // Cancel Loop Coroutine
                    self.token.Cancel();
                    return;
                }
            }
        }

        public static async ETTask<Status> LoopCor(this LoopComponent self)
        {
            BBParser parser = self.GetParent<BBParser>();
            
            //1. 生成Loop协程Id
            long funcId = IdGenerater.Instance.GenerateInstanceId();
            parser.Coroutine_Pointers.Add(funcId, self.startIndex);
            
            //2. 
            parser.CancellationToken.Add(()=>
            { 
                // 取消当前行为协程，对应地取消Loop协程
                self.token.Cancel();
                // 移除Loop协程Id
                parser.Coroutine_Pointers.Remove(funcId);
            });
            
            while (true)
            {
                //3. 逐条执行指令
                while (++parser.Coroutine_Pointers[funcId] < self.endIndex)
                {
                    // 根据OpType匹配Handler
                    string opLine = parser.OpDict[parser.Coroutine_Pointers[funcId]];
                    if (parser.GroupPointerSet.Contains(parser.Coroutine_Pointers[funcId]))
                    {
                        return Status.Failed;
                    }
                    Match match = Regex.Match(opLine, @"^\w+\b(?:\(\))?");
                    if (!match.Success)
                    {
                        Log.Error($"{opLine}匹配失败! 请检查格式");
                        return Status.Failed;
                    }
                    string opType = match.Value;
                    BBScriptHandler scriptHandler = ScriptDispatcherComponent.Instance.GetScriptHandler(opType);
                    if (scriptHandler == null)
                    {
                        Log.Error($"not found script handler: {opType}");
                        return Status.Failed;
                    }
                    
                    // 执行指令
                    BBScriptData data = BBScriptData.Create(opLine, funcId);
                    Status ret = await scriptHandler.Handle(parser, data, self.token);
                    data.Recycle();
                    
                    // Loop协程被取消, 执行下一条指令
                    if (self.token.IsCancel())
                    {
                        return Status.Success;
                    }
                    // 当前行为被取消 or 执行失败
                    if (parser.CancellationToken.IsCancel() || ret != Status.Success)
                    {
                        return Status.Failed;
                    }
                    
                    // 记录Loop协程当前指针
                    self.curIndex = parser.Coroutine_Pointers[funcId];
                }
                
                //4. 跳转头指针重新执行
                parser.Coroutine_Pointers[funcId] = self.startIndex;
                
                //5. 避免卡死
                await TimerComponent.Instance.WaitFrameAsync(self.token);
                if (self.token.IsCancel())
                {
                    return Status.Success;
                }
            }
        }
    }
}