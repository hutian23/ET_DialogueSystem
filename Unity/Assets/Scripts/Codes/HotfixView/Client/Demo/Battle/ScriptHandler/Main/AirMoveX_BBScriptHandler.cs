using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.AirMoveXTimer)]
    public class AirMoveTimer: BBTimer<Unit>
    {
        protected override void Run(Unit self)
        {
            InputWait inputWait = self.GetComponent<InputWait>();
            B2Unit b2Unit = self.GetComponent<B2Unit>();
            BBParser parser = self.GetComponent<BBParser>();
            
            //输入左右相关的指令才会生效水平移动的效果
            bool direction = inputWait.IsPressing(BBOperaType.MIDDLE) || inputWait.IsPressing(BBOperaType.UP) || inputWait.IsPressing(BBOperaType.DOWN);
            
            //当前回中，则不会进行移动
            if (parser.GetParam<bool>("AirMoveX_InertiaEffect") && direction) return;
            parser.UpdateParam("AirMoveX_InertiaEffect", false);
            b2Unit.SetVelocityX(direction ? 0 : parser.GetParam<long>("AirMoveX_Vel") / 10000f);
        }
    }

    public class AirMoveX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AirMoveX";
        }

        //AirMoveX: (水平移动速度,标量)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"AirMoveX: (?<MoveX>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["MoveX"].Value, out long moveX))
            {
                Log.Error($"cannot format {match.Groups["MoveX"].Value} to long");
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            
            //1. 初始化
            parser.TryRemoveParam("AirMoveX_InertiaEffect");
            parser.TryRemoveParam("AirMoveX_Vel");
            if (parser.ContainParam("AirMoveX_Timer"))
            {
                long preTimer = parser.GetParam<long>("AirMoveX_Timer");
                bbTimer.Remove(ref preTimer);
            }
            parser.TryRemoveParam("AirMoveX_Timer");
            
            //注册变量
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.AirMoveXTimer, unit);
            parser.RegistParam("AirMoveX_InertiaEffect", true);
            parser.RegistParam("AirMoveX_Vel", moveX);
            parser.RegistParam("AirMoveX_Timer", timer);
            token.Add(() =>
            {
                bbTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}