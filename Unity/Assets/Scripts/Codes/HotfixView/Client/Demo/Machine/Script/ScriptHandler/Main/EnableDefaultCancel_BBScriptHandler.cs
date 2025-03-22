using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.DefaultCancelTimer)]
    [FriendOf(typeof(BehaviorMachine))]
    [FriendOf(typeof(BehaviorInfo))]
    public class DefaultCancelTimer : BBTimer<Unit>
    {
        protected override void Run(Unit self)
        {
            BehaviorMachine machine = self.GetComponent<BehaviorMachine>();
            BBTimerComponent bbTimer = self.GetComponent<BBTimerComponent>();
            BBParser bbParser = self.GetComponent<BBParser>();

            //1. 
            int currentOrder = machine.GetCurrentOrder();
            for (int i = machine.infoList.Count - 1; i > machine.GetCurrentOrder(); i--)
            {
                BehaviorInfo info = machine.GetChild<BehaviorInfo>(machine.infoList[i]);
                if (info.moveType >= MoveType.Other)
                {
                    continue;
                }
                if (info.Trigger())
                {
                    currentOrder = info.behaviorOrder;
                    break;
                }
            }
            if (currentOrder == machine.GetCurrentOrder())
            {
                return;
            }

            //2. 初始化
            long timer = bbParser.GetParam<long>("DefaultCancel_Timer");
            bbTimer.Remove(ref timer);
            bbParser.TryRemoveParam("DefaultCancel_Timer");

            //3. 进入行为
            machine.Reload(currentOrder);
        }
    }

    public class EnableDefaultCancel_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableDefaultCancel";
        }

        //处于中立状态，可以切换进权值比自己高的动作
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableDefaultCancel: (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Success;
            }
            
            //1. 初始化
            Unit unit = parser.GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();

            if (parser.ContainParam("DefaultCancel_Timer"))
            {
                long _timer = parser.GetParam<long>("DefaultCancel_Timer");
                bbTimer.Remove(ref _timer);
                parser.TryRemoveParam("DefaultCancel_Timer");
            }
            if (match.Groups["Enable"].Value.Equals("false"))
            {
                return Status.Success;
            }
            
            //2. 注册定时器
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.DefaultCancelTimer, unit);
            parser.RegistParam("DefaultCancel_Timer", timer);
            
            token.Add(() =>
            {
                bbTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}