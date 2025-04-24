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
            
            //1. 初始化组件
            parser.RemoveComponent<LoopComponent>();
            LoopComponent loopComponent = parser.AddComponent<LoopComponent>(true);
            loopComponent.triggerIndex = startIndex;
            loopComponent.startIndex = startIndex;
            loopComponent.endIndex = endIndex;
            loopComponent.token = new ETCancellationToken();
            
            //2. 启动检测协程
            loopComponent.TriggerCor().Coroutine();

            //3. 启动Loop协程
            Status ret = await loopComponent.LoopCor();
            parser.RemoveComponent<LoopComponent>();
        
            //4. 跳过代码块
            parser.Coroutine_Pointers[data.CoroutineID] = endIndex;
            
            return ret;
        }
    }
}