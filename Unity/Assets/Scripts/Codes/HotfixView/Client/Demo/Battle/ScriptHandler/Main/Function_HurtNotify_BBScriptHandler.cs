using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(HurtComponent))]
    public class Function_HurtNotify_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HurtNotify";
        }

        //Once: 在判定框持续持续窗口内，对于同一unit只会产生1hit
        //Repeat: 在判定框持续窗口内，对于同一unit每间隔一frame产生1hit
        //HitNotify: Once;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"HurtNotify: (?<CheckType>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //1. 跳过代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opline = parser.OpDict[index];
                if (opline.Equals("EndNotify:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = index;

            //2. 添加受击检测组件
            parser.RemoveComponent<HurtComponent>();
            HurtComponent hurt = parser.AddComponent<HurtComponent>();

            //3. 初始化
            hurt.startIndex = startIndex;
            hurt.endIndex = endIndex;
            hurt.checkType = match.Groups["CheckType"].Value;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}