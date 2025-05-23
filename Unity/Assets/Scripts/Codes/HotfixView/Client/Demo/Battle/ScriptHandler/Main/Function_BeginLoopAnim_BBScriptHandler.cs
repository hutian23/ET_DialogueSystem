namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(LoopAnimComponent))]
    public class Function_BeginLoopAnim_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BeginLoopAnim";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndLoopAnim:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = endIndex;

            //1. 组件初始化
            parser.RemoveComponent<LoopAnimComponent>();
            LoopAnimComponent loopAnimComponent = parser.AddComponent<LoopAnimComponent>(true);
            loopAnimComponent.triggerIndex = startIndex;
            parser.RegistSubCoroutine(startIndex, endIndex, token).Coroutine();

            //2. 
            await loopAnimComponent.LoopAnimCor();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}