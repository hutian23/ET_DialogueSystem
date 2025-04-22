using System.Text.RegularExpressions;

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
            MatchCollection matches = Regex.Matches(data.opLine, @"\((.*?)\)");
            if (matches.Count == 0)
            {
                Log.Error($"Loop_Handler must have at least one triggerHandler!");
                return Status.Failed;
            }

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
            LoopComponent loopComponent = parser.GetComponent<LoopComponent>();
            loopComponent.triggerIndex = startIndex;
            loopComponent.startIndex = startIndex;
            loopComponent.endIndex = endIndex;
            loopComponent.token = new ETCancellationToken();
            
            //2. 启动协程
            
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}