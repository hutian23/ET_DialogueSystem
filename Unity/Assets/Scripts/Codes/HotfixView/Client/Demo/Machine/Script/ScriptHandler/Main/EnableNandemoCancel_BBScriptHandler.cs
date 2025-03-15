using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.NandemoCancelTimer)]
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(BehaviorMachine))]
    public class NandemoCancelTimer : BBTimer<Unit>
    {
        protected override void Run(Unit self)
        {
            BehaviorMachine machine = self.GetComponent<BehaviorMachine>();
            BBTimerComponent bbTimer = self.GetComponent<BBTimerComponent>();
            BBParser parser = self.GetComponent<BBParser>();
            
            //1. 
            int currentOrder = machine.GetCurrentOrder();
            for(int i = machine.infoList.Count - 1; i >= 0; i--)
            {
                BehaviorInfo info = machine.GetChild<BehaviorInfo>(machine.infoList[i]);
                //非控制器层的动作
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
            
            //2.销毁定时器
            long timer = parser.GetParam<long>("NandemoCancel_Timer");
            bbTimer.Remove(ref timer);
            parser.TryRemoveParam("NandemoCancel_Timer");
           
            //3. 重载行为
            machine.Reload(currentOrder);
        }
    }
    
    // 对应IASA，一般用于取消一些过渡动画，当前动作可被所有动作取消
    //EnableNandemoCancel: true;
    public class EnableNandemoCancel_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "EnableNandemoCancel";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"EnableNandemoCancel: (?<Enable>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            //1. 初始化
            Unit unit = parser.GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
           
            if (parser.ContainParam("NandemoCancel_Timer"))
            {
                long _timer = parser.GetParam<long>("NandemoCancel_Timer");
                bbTimer.Remove(ref _timer);
                parser.TryRemoveParam("NandemoCancel_Timer");
            }
            if (match.Groups["Enable"].Value.Equals("false"))
            {
                return Status.Success;
            }
            
            //2. 注册定时器
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.NandemoCancelTimer, unit);
            parser.RegistParam("NandemoCancel_Timer", timer);
            
            token.Add(() =>
            {
                bbTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}