namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(LoopComponent))]
    public class Function_BeginLoop_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BeginLoop";
        }

        //Loop: (Transition: Squat, true), (InAir: true)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, triggerIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndLoop:"))
                {
                    endIndex = index;
                    break;
                }
            }
            
            //1. 初始化组件
            parser.RemoveComponent<LoopComponent>();
            LoopComponent loopComponent = parser.AddComponent<LoopComponent, int, int>(triggerIndex, endIndex, true);
            
            //2. 启动Loop协程
            Status ret = await loopComponent.LoopCor();
            if (token.IsCancel()) return Status.Failed;
        
            //3. 跳过代码块
            parser.RemoveComponent<LoopComponent>();
            parser.Coroutine_Pointers[data.CoroutineID] = endIndex;
            
            //4. 下一逻辑帧，才执行下一条指令
            await BBTimerManager.Instance.LateUpdateTimer().WaitFrameAsync(token);
            if (token.IsCancel()) return Status.Failed;
            
            return ret;
        }
    }
}