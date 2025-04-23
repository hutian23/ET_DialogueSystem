namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(LoopComponent))]
    public class Function_Loop_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Loop";
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
            //1. 跳过代码块
            parser.Coroutine_Pointers[data.CoroutineID] = endIndex;
            
            //2. 初始化组件
            parser.RemoveComponent<LoopComponent>();
            LoopComponent loopComponent = parser.AddComponent<LoopComponent>();
            loopComponent.triggerIndex = startIndex;
            loopComponent.startIndex = startIndex;
            loopComponent.endIndex = endIndex;
            loopComponent.token = new ETCancellationToken();
            
            //3. 启动检测协程
            loopComponent.TriggerCor().Coroutine();

            //4. 启动Loop协程
            Status ret = await loopComponent.LoopCor();
            loopComponent.Dispose();
            return ret;
        }
    }
}