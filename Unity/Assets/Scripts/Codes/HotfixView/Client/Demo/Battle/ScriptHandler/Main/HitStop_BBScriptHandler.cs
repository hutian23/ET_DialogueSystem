using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.HitStopTimer)]
    public class HitStopTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            BBNumeric numeric = self.GetParent<Unit>().GetComponent<BBNumeric>();
            
            int cnt = self.GetParam<int>("HitStop_Cnt");
            long timer = self.GetParam<long>("HitStop_Timer");
            
            //更新计时器
            self.UpdateParam("HitStop_Cnt", --cnt);
            if (cnt > 0)
            {
                return;
            }
            
            //初始化
            numeric.Set("Hertz", 60);
            sceneTimer.Remove(ref timer);
            self.TryRemoveParam("HitStop_Timer");
            self.TryRemoveParam("HitStop_Cnt");
        }
    }

    [FriendOf(typeof(BBParser))]
    public class HitStop_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitStop";
        }

        //HitStop: 6, 8;(Hertz, hitStopFrame)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "HitStop: (?<TimeScale>.*?), (?<HitStop>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["TimeScale"].Value, out int hertz))
            {
                Log.Error($"cannot format timeScale to int!");
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["HitStop"].Value, out int hitStop))
            {
                Log.Error($"cannot format HitStop to int!");
                return Status.Failed;
            }
            if (hitStop == 0)
            {
                return Status.Success;
            }

            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            BBNumeric numeric = parser.GetParent<Unit>().GetComponent<BBNumeric>();
            
            //1. 初始化
            if (parser.ContainParam("HitStop_Timer"))
            {
                long _timer = parser.GetParam<long>("HitStop_Timer");
                sceneTimer.Remove(ref _timer);
                parser.TryRemoveParam("HitStopTimer");
            }
            parser.TryRemoveParam("HitStop_Cnt");
            
            //2. 注册定时任务
            long timer = sceneTimer.NewFrameTimer(BBTimerInvokeType.HitStopTimer, parser);
            parser.RegistParam("HitStop_Timer", timer);
            parser.RegistParam("HitStop_Cnt", hitStop);
            
            numeric.Set("Hertz", hertz);
            token.Add(() =>
            {
                numeric.Set("Hertz", 60);
                sceneTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}