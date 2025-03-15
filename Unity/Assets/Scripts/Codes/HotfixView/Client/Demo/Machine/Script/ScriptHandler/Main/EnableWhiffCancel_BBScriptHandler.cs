using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.WhiffCancelTimer)]
    [FriendOf(typeof(BehaviorMachine))]
    [FriendOf(typeof(BehaviorInfo))]
    public class WhiffCancelTimer : BBTimer<Unit>
    {
        protected override void Run(Unit self)
        {
            BehaviorMachine machine = self.GetComponent<BehaviorMachine>();
            BBTimerComponent bbTimer = self.GetComponent<BBTimerComponent>();
            BBParser bbParser = self.GetComponent<BBParser>();
            BehaviorInfo curInfo = machine.GetInfoByOrder(machine.GetCurrentOrder());

            //1. 找到能够取消的行为
            int currentOrder = -1;
            HashSetComponent<string> options = bbParser.GetParam<HashSetComponent<string>>("WhiffCancel_Options");
            foreach (string whiffOption in options)
            {
                BehaviorInfo info = machine.GetInfoByName(whiffOption);
                //当前挥空取消 不能取消进 非控制器层动作 || 待机动作 || 自己
                if (info == null || info.moveType >= MoveType.Other || info.behaviorOrder == 0 || info.behaviorOrder == curInfo.behaviorOrder)
                {
                    continue;
                }

                if (info.Trigger())
                {
                    currentOrder = info.behaviorOrder;
                }
            } 
            if (currentOrder != -1)
            {
                return;
            }
            

            //2. 关闭取消窗口
            long timer = bbParser.GetParam<long>("WhiffCancel_Timer");
            bbTimer.Remove(ref timer);
            bbParser.TryRemoveParam("WhiffCancel_Timer");
            options.Dispose();
            bbParser.TryRemoveParam("WhiffCancel_Options");

            //3. 进入行为
            machine.Reload(currentOrder);
        }
    }

    public class EnableWhiffCancel_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableWhiffCancel";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableWhiffCancel: (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Success;
            }
            
            //1.初始化
            Unit unit = parser.GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();

            if (parser.ContainParam("WhiffCancel_Timer"))
            {
                long _timer = parser.GetParam<long>("WhiffCancel_Timer");
                bbTimer.Remove(ref _timer);
                parser.TryRemoveParam("WhiffCancel_Timer");
            }
            if (match.Groups["Enable"].Value.Equals("false"))
            {
                return Status.Success;
            }
            
            //2. 注册定时器
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.WhiffCancelTimer, unit);
            HashSetComponent<string> hashSetComponent = HashSetComponent<string>.Create();
            parser.RegistParam("WhiffCancel_Timer", timer);
            parser.RegistParam("WhiffCancel_Options", hashSetComponent);
            
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