using System.Collections.Generic;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.ThrowCheckTimer)]
    [FriendOf(typeof(B2Unit))]
    [FriendOf(typeof(BBParser))]
    public class ThrowCheckTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            B2Unit b2Unit = self.GetParent<Unit>().GetComponent<B2Unit>();

            Queue<CollisionInfo> infoQueue = b2Unit.TriggerBuffer;
            int count = infoQueue.Count;

            while (count-- > 0)
            {
                CollisionInfo info = infoQueue.Dequeue();
                infoQueue.Enqueue(info);

                BoxInfo infoA = info.dataA.UserData as BoxInfo;
                BoxInfo infoB = info.dataB.UserData as BoxInfo;
                if (infoA.hitboxType is HitboxType.Throw && infoB.hitboxType is HitboxType.Squash)
                {
                    //here: 注册变量
                    self.TryRemoveParam("TargetBind_ThrowHurt");
                    self.TryRemoveParam("SubTimeline_ThrowHurt");
                    self.TryRemoveParam("Hurt_CollisionInfo");
                    
                    self.RegistParam("Hurt_CollisionInfo", info); // 调用hitStun指令
                    self.RegistParam("TargetBind_ThrowHurt", info.dataB.InstanceId); // b2BodyId
                    self.RegistParam("SubTimeline_ThrowHurt", info.dataB.InstanceId);
                    
                    //子携程
                    int startIndex = self.GetParam<int>("ThrowCheck_StartIndex");
                    int endIndex = self.GetParam<int>("ThrowCheck_EndIndex");
                    self.RegistSubCoroutine(startIndex, endIndex, self.CancellationToken).Coroutine();

                    //初始化
                    long timer = self.GetParam<long>("ThrowCheck_Timer");
                    postStepTimer.Remove(ref timer);
                    self.TryRemoveParam("ThrowCheckTimer");
                    self.TryRemoveParam("ThrowCheck_StartIndex");
                    self.TryRemoveParam("ThrowCheck_EndIndex");
                    break;
                }
            }
        }
    }
    [FriendOf(typeof(BBParser))]
    public class WaitThrow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "WaitThrow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();

            // 初始化
            parser.TryRemoveParam("ThrowCheck_StartIndex");
            parser.TryRemoveParam("ThrowCheck_EndIndex");
            if (parser.ContainParam("ThrowCheck_Timer"))
            {
                long preTimer = parser.GetParam<long>("ThrowCheck_Timer");
                postStepTimer.Remove(ref preTimer);
            }
            parser.TryRemoveParam("ThrowCheck_Timer");
            
            //跳过代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndThrow:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = index;
            
            // 注册定时器
            long timer = postStepTimer.NewFrameTimer(BBTimerInvokeType.ThrowCheckTimer, parser);
            parser.RegistParam("ThrowCheck_Timer", timer);
            parser.RegistParam("ThrowCheck_StartIndex", startIndex);
            parser.RegistParam("ThrowCheck_EndIndex", endIndex);
            
            token.Add(() =>
            {
                postStepTimer.Remove(ref timer);
            });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}