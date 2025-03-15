using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.GatlingCancelTimer)]
    [FriendOf(typeof(BehaviorMachine))]
    [FriendOf(typeof(BehaviorInfo))]
    public class GatlingCancelTimer : BBTimer<Unit>
    {
        protected override void Run(Unit self)
        {
            //1. 找到能够取消的行为
            BehaviorMachine machine = self.GetComponent<BehaviorMachine>();
            BBTimerComponent bbTimer = self.GetComponent<BBTimerComponent>();
            BBParser bbParser = self.GetComponent<BBParser>();
            
            int currentOrder = -1;
            BehaviorInfo curInfo = machine.GetInfoByOrder(machine.GetCurrentOrder());
            HashSetComponent<string> options = bbParser.GetParam<HashSetComponent<string>>("GatlingCancel_Options");
            
            for(int i = machine.infoList.Count - 1; i >= 0; i--)
            {
                BehaviorInfo info = machine.GetChild<BehaviorInfo>(machine.infoList[i]);
                if (info.moveType >= MoveType.Other)
                {
                    continue;
                }
                //只能被同层的 or 添加了CancelTag的动作取消
                if ((info.moveType > curInfo.moveType || options.Contains(info.behaviorName)) && info.Trigger())
                {
                    currentOrder = info.behaviorOrder;
                    break;
                }
            }
            if (currentOrder == -1)
            {
                return;
            }

            //2. 关闭取消窗口
            long timer = bbParser.GetParam<long>("GatlingCancel_Timer");
            bbTimer.Remove(ref timer);
            bbParser.TryRemoveParam("CancelCancel_Timer");
            options.Dispose();
            bbParser.TryRemoveParam("CancelCancel_Options");
            
            //3. 进入行为
            machine.Reload(currentOrder);
        }
    }
    
    // 加特林取消，当前动作只能被比自己层级高 or 添加了取消标签的动作取消
    public class EnableGatlingCancel_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableGatlingCancel";
        }

        //EnableGatlingCancel: true;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableGatlingCancel: (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Success;
            }
            
            //1. 初始化
            Unit unit = parser.GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();

            if (parser.ContainParam("GatlingCancel_Timer"))
            {
                long _timer = parser.GetParam<long>("GatlingCancel_Timer");
                bbTimer.Remove(ref _timer);
                parser.TryRemoveParam("GatlingCancel_Timer");
            }
            if (parser.ContainParam("GatlingCancel_Options"))
            {
                HashSetComponent<string> GCOptions = parser.GetParam<HashSetComponent<string>>("GatlingCancel_Options");
                GCOptions.Dispose();
                parser.TryRemoveParam("GatlingCancel_Options");
            }
            if (match.Groups["Enable"].Value.Equals("false"))
            {
                return Status.Success;
            }
            
            //2. 注册定时器
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.GatlingCancelTimer, unit);
            HashSetComponent<string> hashSetComponent = HashSetComponent<string>.Create();
            parser.RegistParam("GatlingCancel_Timer", timer);
            parser.RegistParam("GatlingCancel_Options", hashSetComponent);
            
            token.Add(() =>
            {
                bbTimer.Remove(ref timer);
                hashSetComponent?.Dispose();
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}