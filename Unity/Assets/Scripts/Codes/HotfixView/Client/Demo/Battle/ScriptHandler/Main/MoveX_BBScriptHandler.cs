using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.MoveXTimer)]
    public class MoveXTimer: BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            B2Unit b2Unit = self.GetParent<Unit>().GetComponent<B2Unit>();
            float v = self.GetParam<float>("MoveX_Vel");
            b2Unit.SetVelocityX(v);
        }
    }

    //对于 run AirBone这些行为，需要在行为中实时转向并改变速度
    public class MoveX_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "MoveX";
        }

        //MoveX: 8.3;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //正则匹配成员变量
            Match match = Regex.Match(data.opLine, @"MoveX: (?<MoveX>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["MoveX"].Value, out long moveX))
            {
                Log.Error($"cannot format {match.Groups["MoveX"].Value} to long!!!");
                return Status.Failed;
            }

            BBTimerComponent bbTimer = parser.GetParent<Unit>().GetComponent<BBTimerComponent>();
            
            //初始化
            parser.TryRemoveParam("MoveX_Vel");
            if (parser.ContainParam("MoveX_Timer"))
            {
                long _timer = parser.GetParam<long>("MoveX_Timer");
                bbTimer.Remove(ref _timer);
            }
            parser.TryRemoveParam("MoveX_Timer");
            
            //注册变量
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.MoveXTimer, parser); 
            parser.RegistParam("MoveX_Vel", moveX / 10000f);
            parser.RegistParam("MoveX_Timer", timer);
            
            token.Add(() =>
            {
                bbTimer.Remove(ref timer);
            });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}