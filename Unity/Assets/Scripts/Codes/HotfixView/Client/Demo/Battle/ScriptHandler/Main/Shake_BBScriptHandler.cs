using System.Text.RegularExpressions;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.ShakeTimer)]
    [FriendOf(typeof(b2Body))]
    public class ShakeTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            Unit unit = self.GetParent<Unit>();
            b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();

            float shakeLength_X = self.GetParam<float>("Shake_LengthX");
            float shakeLength_Y = self.GetParam<float>("Shake_LengthY");
            float frequency = self.GetParam<float>("Shake_Frequency");
            int curFrame = self.GetParam<int>("Shake_Frame");
            int totalFrame = self.GetParam<int>("Shake_TotalFrame");
            long timer = self.GetParam<long>("Shake_Timer");
            System.Random _ran = new();
            
            Vector2 shakePos = new Vector2(shakeLength_X, shakeLength_Y) *  // 振幅
                    new Vector2(_ran.Next(60, 120), _ran.Next(60, 120)) / 100f * // 噪声 
                    new Vector2(Mathf.Cos(curFrame * frequency ) * (curFrame / (float)totalFrame), // 频率
                                Mathf.Sin(curFrame * frequency ) * (curFrame / (float)totalFrame)); 
            
            //通知逻辑层更新渲染层
            b2Body.UpdateFlag = true;
            b2Body.offset = shakePos;
            
            //2. 振动效果执行完毕?
            curFrame--;
            //销毁相关变量
            if (curFrame <= 0)
            {
                postStepTimer.Remove(ref timer);
                self.TryRemoveParam("Shake_Timer");
                self.TryRemoveParam("Shake_Frame");
                self.TryRemoveParam("Shake_TotalFrame");
                self.TryRemoveParam("Shake_Frequency");
                self.TryRemoveParam("Shake_LengthX");
                self.TryRemoveParam("Shake_LengthY");
                return;
            }
            
            //3. 更新计时器
            self.UpdateParam("Shake_Frame", curFrame);
        }
    }

    [FriendOf(typeof(b2Body))]
    public class Shake_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Shake";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Shake: (?<ShakeLength_X>.*?), (?<ShakeLength_Y>.*?), (?<Frequency>.*?), (?<ShakeFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["ShakeLength_X"].Value, out long shakeLength_X) ||
                !long.TryParse(match.Groups["ShakeLength_Y"].Value, out long shakeLength_Y) ||
                !int.TryParse(match.Groups["ShakeFrame"].Value, out int shakeFrame) ||
                !long.TryParse(match.Groups["Frequency"].Value, out long frequency))
            {
                Log.Error($"cannot format {match.Groups["ShakeFrame"].Value} / {match.Groups["ShakeLength_X"].Value} / {match.Groups["ShakeLength_Y"].Value} / {match.Groups["Frequency"].Value} to long!!");
                return Status.Failed;
            }

            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            b2Body b2Body = b2WorldManager.Instance.GetBody(parser.GetParent<Unit>().InstanceId);

            //1. 初始化
            parser.TryRemoveParam("Shake_LengthX");
            parser.TryRemoveParam("Shake_LengthY");
            parser.TryRemoveParam("Shake_Frame");
            parser.TryRemoveParam("Shake_TotalFrame");
            parser.TryRemoveParam("Shake_Frequency");
            if (parser.ContainParam("Shake_Timer"))
            {
                long _timer = parser.GetParam<long>("Shake_Timer");
                postStepTimer.Remove(ref _timer);
            }
            parser.TryRemoveParam("Shake_Timer");

            //2. 注册变量
            long timer = postStepTimer.NewFrameTimer(BBTimerInvokeType.ShakeTimer, parser);
            parser.RegistParam("Shake_LengthX", shakeLength_X / 10000f);
            parser.RegistParam("Shake_LengthY", shakeLength_Y / 10000f);
            parser.RegistParam("Shake_Frequency", frequency / 10000f);
            parser.RegistParam("Shake_Frame", shakeFrame);
            parser.RegistParam("Shake_TotalFrame", shakeFrame);
            parser.RegistParam("Shake_Timer", timer);

            //3. 切换行为时销毁振动效果
            token.Add(() =>
            {
                postStepTimer.Remove(ref timer);
                b2Body.UpdateFlag = true;
            });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}