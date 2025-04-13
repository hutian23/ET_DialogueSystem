using System.Collections.Generic;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.HitCheckTimer)]
    [FriendOf(typeof(B2Unit))]
    [FriendOf(typeof(BBParser))]
    public class HitCheckTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            B2Unit b2Unit = self.GetParent<Unit>().GetComponent<B2Unit>();

            //PostStep生命周期中，取出碰撞缓冲区中碰撞信息
            Queue<CollisionInfo> infoQueue = b2Unit.TriggerBuffer;
            int count = infoQueue.Count;

            bool wasHit = false;
            while (count-- > 0)
            {
                CollisionInfo info = infoQueue.Dequeue();
                infoQueue.Enqueue(info);

                BoxInfo infoA = info.dataA.UserData as BoxInfo;
                BoxInfo infoB = info.dataB.UserData as BoxInfo;
                if (infoA.hitboxType is HitboxType.Hit && infoB.hitboxType is HitboxType.Hurt)
                {
                    wasHit = true;
                    break;
                }
            }
            if (!wasHit) return;
            
            //执行回调(子协程)
            int startIndex = self.GetParam<int>("HitCheck_StartIndex");
            int endIndex = self.GetParam<int>("HitCheck_EndIndex");
            self.RegistSubCoroutine(startIndex, endIndex, self.CancellationToken).Coroutine();
            
            //初始化
            long timer = self.GetParam<long>("HitCheckTimer");
            postStepTimer.Remove(ref timer);
            self.TryRemoveParam("HitCheckTimer");
            self.TryRemoveParam("HitCheck_StartIndex");
            self.TryRemoveParam("HitCheck_EndIndex");   
        }
    }

    [FriendOf(typeof(BBParser))]
    public class WaitHit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "WaitHit";
        }

        //WaitHit: Once / Repeat
        //打击框和受击框重叠，调用攻击回调，执行卡肉，回复能量等逻辑
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 初始化
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            if (parser.ContainParam("HitCheckTimer"))
            {
                long preTimer = parser.GetParam<long>("HitCheckTimer");
                postStepTimer.Remove(ref preTimer);
            }
            parser.TryRemoveParam("HitCheckTimer");
            parser.TryRemoveParam("HitCheck_StartIndex");
            parser.TryRemoveParam("HitCheck_EndIndex");
            
            //2. 跳过代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opLine = parser.OpDict[index];
                if (opLine.Equals("EndHit:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = index;
            
            //3. PostStep生命周期每帧检测
            long timer = postStepTimer.NewFrameTimer(BBTimerInvokeType.HitCheckTimer, parser);
            parser.RegistParam("HitCheck_StartIndex", startIndex);
            parser.RegistParam("HitCheck_EndIndex", endIndex);
            parser.RegistParam("HitCheckTimer", timer);
            
            token.Add(() =>
            {
                postStepTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}