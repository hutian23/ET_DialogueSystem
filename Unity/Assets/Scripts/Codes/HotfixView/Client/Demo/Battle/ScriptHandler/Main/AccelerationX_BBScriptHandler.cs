using System;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.AccelerationXTimer)]
    public class AccelerationXTimer: BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            B2Unit b2Unit = self.GetParent<Unit>().GetComponent<B2Unit>();
            
            float ax = self.GetParam<long>("AccelerationX_Accel") / 10000f;
            float maxV = self.GetParam<long>("AccelerationX_MaxV") / 10000f;
            float dv = (1 / 60f) * ax;
            
            float curV = b2Unit.GetVelocity().X + dv;
            //限制速度大小
            b2Unit.SetVelocityX( Math.Abs(curV) >= maxV? curV = Math.Sign(curV) * maxV : curV);
        }
    }
    
    public class AccelerationX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AccelerationX";
        }

        //AccelerationX: a, MaxV(矢量);
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "AccelerationX: (?<A>.*?), (?<MaxV>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["A"].Value, out long accelerationX) || !long.TryParse(match.Groups["MaxV"].Value, out long maxV))
            {
                Log.Error($"cannot format {match.Groups["A"].Value} / {match.Groups["MaxV"].Value} to long!!!");
                return Status.Failed;
            }

            BBTimerComponent bbTimer = parser.GetParent<Unit>().GetComponent<BBTimerComponent>();
            
            //1. 初始化
            if (parser.ContainParam("AccelerationX_Timer"))
            {
                long timer = parser.GetParam<long>("AccelerationX_Timer");
                bbTimer.Remove(ref timer);
            }
            parser.TryRemoveParam("AccelerationX_Timer");
            parser.TryRemoveParam("AccelerationX_Accel");
            parser.TryRemoveParam("AccelerationX_MaxV");
            
            //2. 注册变量，启动定时器
            parser.RegistParam("AccelerationX_Accel", accelerationX);
            parser.RegistParam("AccelerationX_MaxV", maxV);
            long curTimer = bbTimer.NewFrameTimer(BBTimerInvokeType.AccelerationXTimer, parser);
            parser.RegistParam("AccelerationX_Timer", curTimer);
            
            token.Add(() =>
            {
                bbTimer.Remove(ref curTimer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}