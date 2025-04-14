using System.Collections.Generic;
using System.Text.RegularExpressions;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.BounceCheckTimer)]
    [FriendOf(typeof(B2Unit))]
    public class BounceCheckTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            B2Unit b2Unit = self.GetParent<Unit>().GetComponent<B2Unit>();
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();    
            
            Queue<CollisionInfo> infoQueue = b2Unit.TriggerBuffer;
            int count = infoQueue.Count;
            
            while (count -- > 0)
            {
                CollisionInfo info = infoQueue.Dequeue();
                infoQueue.Enqueue(info);

                BoxInfo infoA = info.dataA.UserData as BoxInfo;
                if (infoA.boxName.Equals("BounceCheckBox") && 
                    infoA.hitboxType is HitboxType.Other &&
                    info.dataB.LayerMask is LayerType.Ground &&
                    info.dataB.InstanceId != 0)
                {
                    self.UpdateParam("BounceCheck_Flag", true);
                    
                    //初始化
                    long timer = self.GetParam<long>("BounceCheck_Timer");
                    postStepTimer.Remove(ref timer);
                    self.TryRemoveParam("BounceCheck_Timer");
                    self.TryRemoveParam("BounceCheck_WaitFrame");
                    return;
                }
            }
        }
    }

    public class BounceCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BounceCheck";
        }

        //BounceCheck: true, 10; (开启弹墙检查，弹墙检查持续多少帧)
        // 需要在timeline编辑器中添加BounceCheckBox
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"BounceCheck: (?<Flag>\w+), (?<WaitFrame>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["WaitFrame"].Value, out int waitFrame))
            {
                Log.Error($"cannot format {match.Groups["WaitFrame"].Value} to int!!");
                return Status.Failed;
            }
            
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            //1. 初始化
            if (parser.ContainParam("BounceCheck_Timer"))
            {
                long _timer = parser.GetParam<long>("BounceCheck_Timer");
                postStepTimer.Remove(ref _timer);
            }
            parser.TryRemoveParam("BounceCheck_Timer");
            parser.TryRemoveParam("BounceCheck_Flag");
            parser.TryRemoveParam("BounceCheck_WaitFrame");
            if (match.Groups["Flag"].Value.Equals("false")) return Status.Success;
            
            //2. 注册定时器
            long timer = postStepTimer.NewFrameTimer(BBTimerInvokeType.BounceCheckTimer, parser);
            parser.RegistParam("BounceCheck_Timer", timer);
            parser.RegistParam("BounceCheck_Flag", false);
            parser.RegistParam("BounceCheck_WaitFrame", waitFrame);
            
            token.Add(() =>
            {
                postStepTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}