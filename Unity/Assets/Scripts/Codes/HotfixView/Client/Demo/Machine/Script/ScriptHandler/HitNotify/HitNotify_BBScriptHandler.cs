using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(HitComponent))]
    public class HitNotify_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitNotify";
        }

        //Once: 在判定框持续持续窗口内，对于同一unit只会产生1hit
        //Repeat: 在判定框持续窗口内，对于同一unit每间隔一frame产生1hit
        //HurtNotify: Once;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"HitNotify: (?<CheckType>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //1. 跳过这个代码块
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

            //2. 添加攻击检测组件
            parser.RemoveComponent<HitComponent>(); //移除旧的攻击回调
            HitComponent hit = parser.AddComponent<HitComponent>();

            //3. 组件数据初始化
            hit.startIndex = startIndex;
            hit.endIndex = endIndex;
            hit.timer = b2WorldManager.Instance.GetPostStepTimer().NewFrameTimer(BBTimerInvokeType.HitNotifyTimer, parser);
            hit.checkType =match.Groups["CheckType"].Value;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}