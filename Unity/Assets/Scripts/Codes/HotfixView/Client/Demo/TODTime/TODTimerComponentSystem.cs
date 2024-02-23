using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof (TODTimerComponent))]
    public static class TODTimerComponentSystem
    {
        public class TODTimerComponentUpdateSystem: UpdateSystem<TODTimerComponent>
        {
            protected override void Update(TODTimerComponent self)
            {
                self.TimerUpdate();
            }
        }
        
        public class TODTimerComponentDestorySystem: DestroySystem<TODTimerComponent>
        {
            protected override void Destroy(TODTimerComponent self)
            {
                self.Init();
            }
        }

        private static long GetId(this TODTimerComponent self)
        {
            return ++self.idGenerator;
        }

        public static long GetNow(this TODTimerComponent self)
        {
            return self.curFrame;
        }

        private static void Init(this TODTimerComponent self)
        {
            //回收所有定时器
            foreach (var action in self.timerActions.Values)
            {
                action?.Recycle();
            }

            self.TimerId.Clear();
            self.timeOutTime.Clear();
            self.timeOutTimerIds.Clear();
            self.timerActions.Clear();

            self.timeScale = 1f;
            self.minFrame = long.MaxValue;
            self.curFrame = 0;
            self.deltaTimereminder = 0f;
        }

        /// <summary>
        /// 获得一帧的真实时长
        /// </summary>
        private static float GetFrameLength(this TODTimerComponent self)
        {
            //假设一秒为60帧
            return Mathf.Round(1000 / (60 * self.timeScale));
        }

        private static void TimerUpdate(this TODTimerComponent self)
        {
            //时间完全静止了
            if (self.timeScale == 0)
            {
                return;
            }

            self.deltaTimereminder += Time.deltaTime * 1000;

            float frameLength = self.GetFrameLength();
            int num = (int)(self.deltaTimereminder / frameLength);
            self.deltaTimereminder -= num * frameLength;
            self.curFrame += num;

            //当前帧没有可执行的定时器，就不进行遍历了
            if (self.curFrame < self.minFrame)
            {
                return;
            }

            foreach (KeyValuePair<long, List<long>> kv in self.TimerId)
            {
                //key 为帧号
                long k = kv.Key;
                // 设置定时器中的最小执行帧号
                if (k > self.curFrame)
                {
                    self.minFrame = k;
                    break;
                }

                self.timeOutTime.Enqueue(k);
            }

            while (self.timeOutTime.Count > 0)
            {
                long time = self.timeOutTime.Dequeue();
                var list = self.TimerId[time];
                for (int i = 0; i < list.Count; i++)
                {
                    long timerId = list[i];
                    self.timeOutTimerIds.Enqueue(timerId);
                }

                self.TimerId.Remove(time);
            }

            while (self.timeOutTimerIds.Count > 0)
            {
                long timerId = self.timeOutTimerIds.Dequeue();

                if (!self.timerActions.Remove(timerId, out TODTimerAction timerAction))
                {
                    continue;
                }

                self.Run(timerAction);
            }
        }

        private static void Run(this TODTimerComponent self, TODTimerAction timerAction)
        {
            switch (timerAction.TimerClass)
            {
                case TimerClass.OnceTimer:
                    EventSystem.Instance.Invoke(timerAction.Type, new TODTimerCallback() { Args = timerAction.Object });
                    timerAction.Recycle();
                    break;
                case TimerClass.OnceWaitTimer:
                {
                    ETTask tcs = timerAction.Object as ETTask;
                    tcs.SetResult();
                    timerAction.Recycle();
                    break;
                }
                case TimerClass.RepeatedTimer:
                {
                    timerAction.startFrame = self.curFrame;
                    self.AddTimer(timerAction);
                    EventSystem.Instance.Invoke(timerAction.Type, new TODTimerCallback() { Args = timerAction.Object });
                    break;
                }
            }
        }

        private static void AddTimer(this TODTimerComponent self, TODTimerAction timer)
        {
            long tillFrame = timer.startFrame + timer.Frame;
            self.TimerId.Add(tillFrame, timer.Id);
            self.timerActions.Add(timer.Id, timer);

            if (tillFrame < self.minFrame)
            {
                self.minFrame = tillFrame;
            }
        }

        public static bool Remove(this TODTimerComponent self, ref long id)
        {
            long i = id;
            id = 0;
            return self.Remove(i);
        }

        private static bool Remove(this TODTimerComponent self, long id)
        {
            if (id == 0)
            {
                return false;
            }

            if (!self.timerActions.Remove(id, out TODTimerAction timerAction))
            {
                Log.Warning("不存在该定时器");
                return false;
            }

            timerAction.Recycle();
            return true;
        }

        public static async ETTask WaitTillAsync(this TODTimerComponent self, long tillFrame, ETCancellationToken token = null)
        {
            // 传入的帧号要比当前的小
            if (self.curFrame >= tillFrame)
            {
                return;
            }

            ETTask tcs = ETTask.Create(true);
            //将回调传入ETTask,Upate中取出 并执行回调
            TODTimerAction timer = TODTimerAction.Create(self.GetId(),
                TimerClass.OnceWaitTimer,
                self.curFrame,
                tillFrame - self.curFrame,
                0,
                tcs);

            self.AddTimer(timer);
            long timerId = timer.Id;

            void CancelAction()
            {
                if (self.Remove(timerId))
                {
                    tcs.SetResult();
                }
            }

            try
            {
                token?.Add(CancelAction);
                await tcs;
            }
            finally
            {
                token?.Remove(CancelAction);
            }
        }

        public static async ETTask WaitAsync(this TODTimerComponent self, long frame, ETCancellationToken token)
        {
            if (frame == 0)
            {
                return;
            }

            //从对象池中取出ETTask
            ETTask tcs = ETTask.Create(true);
            TODTimerAction timer = TODTimerAction.Create(self.GetId(), TimerClass.OnceWaitTimer, self.curFrame, frame, 0, tcs);
            self.AddTimer(timer);
            long timerId = timer.Id;

            void CancelAction()
            {
                if (self.Remove(timerId))
                {
                    tcs.SetResult();
                }
            }

            try
            {
                token?.Add(CancelAction);
                await tcs;
            }
            finally
            {
                token?.Remove(CancelAction);
            }
        }

        public static async ETTask WaitFrameAsync(this TODTimerComponent self, ETCancellationToken token = null)
        {
            await self.WaitAsync(1, token);
        }

        public static long NewOnceTimer(this TODTimerComponent self, long tillFrame, int type, object args)
        {
            if (tillFrame < self.curFrame)
            {
                Log.Error($"tillframe should be bigger than currentFrame:{tillFrame} {self.curFrame}");
            }

            TODTimerAction timer = TODTimerAction.Create(self.GetId(), TimerClass.OnceTimer, self.curFrame, tillFrame - self.curFrame, type, args);
            self.AddTimer(timer);
            return timer.Id;
        }

        public static long NewFrameTimer(this TODTimerComponent self, int type, object args)
        {
            return self.NewRepeatedTimer(1, type, args);
        }

        private static long NewRepeatedTimer(this TODTimerComponent self, long frame, int type, object args)
        {
            TODTimerAction timer = TODTimerAction.Create(self.GetId(), TimerClass.RepeatedTimer, self.curFrame, frame, type, args);

            self.AddTimer(timer);
            return timer.Id;
        }
    }
}