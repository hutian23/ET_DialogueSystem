using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(ScriptDispatcherComponent))]
    [FriendOf(typeof(BehaviorMachine))]
    public class RootInit_RegistMove_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistMove";
        }

        //RegistMove: (Hello World)
        //    MoveType: Move;
        //    EndMove:
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 匹配BehaviorName
            Match match = Regex.Match(data.opLine, @"RegistMove: \((?<behaviorName>\w+)\)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            BehaviorMachine machine = parser.GetParent<Unit>().GetComponent<BehaviorMachine>();
            
            //2. 注册BehaviorInfo组件到行为机中
            BehaviorInfo info = machine.AddChild<BehaviorInfo>();
            int order = machine.Children.Count - 1;
            info.behaviorOrder = order;
            info.behaviorName = match.Groups["behaviorName"].Value;
            machine.behaviorNameMap.Add(info.behaviorName,info.Id); //快速访问到组件
            machine.behaviorOrderMap.Add(info.behaviorOrder, info.Id);
            machine.infoList.Add(info.Id);
            
            //3. 跳过Move代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndMove:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = index;
            
            //4. RegistMove代码块作为子协程执行
            parser.RegistParam("InfoId", info.Id);
            await parser.RegistSubCoroutine(startIndex, endIndex, token);
            if (token.IsCancel()) return Status.Failed;
            parser.TryRemoveParam("InfoId");
            
            return Status.Success;
        }
    }
}