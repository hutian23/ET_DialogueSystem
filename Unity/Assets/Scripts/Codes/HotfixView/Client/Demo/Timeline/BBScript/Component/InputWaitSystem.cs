using Sirenix.Utilities;

namespace ET.Client
{
    [FriendOf(typeof (InputWait))]
    public static class InputWaitSystem
    {
        [Invoke(BBTimerInvokeType.BBInputTimer)]
        [FriendOf(typeof (BBInputComponent))]
        [FriendOf(typeof (InputWait))]
        public class BBInputTimer: BBTimer<InputWait>
        {
            protected override void Run(InputWait self)
            {
                long ops = BBInputComponent.Instance.Ops;
                self.Notify(ops);

                //检测执行完的输入协程，重新执行
                foreach (var handler in self.handlers)
                {
                    if (!self.runningHandlers.Contains(handler.GetInputType()))
                    {
                        self.InputCheckCor(handler, self.Token).Coroutine();
                    }
                }

                //更新缓冲区
                self.UpdateBuffer();
            }
        }

        private static void Init(this InputWait self)
        {
            self.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>().Remove(ref self.Timer);
            self.Ops = 0;

            self.handlers.Clear();
            self.runningHandlers.Clear();
            self.bufferDict.Clear();

            self.Token?.Cancel();
            self.tcss.ForEach(tcs => tcs.Recycle());
            self.tcss.Clear();
            self.Token = new ETCancellationToken();

            self.bufferQueue.ForEach(buffer => buffer.Recycle());
            self.bufferQueue.Clear();
        }

        public static void Reload(this InputWait self)
        {
            self.Init();

            //启动定时器
            BBTimerComponent bbTimer = self.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
            self.Timer = bbTimer.NewFrameTimer(BBTimerInvokeType.BBInputTimer, self);
        }

        //https://www.zhihu.com/question/36951135/answer/69880133
        private static void Notify(this InputWait self, long op)
        {
            for (int i = 0; i < self.tcss.Count; i++)
            {
                BBTimerComponent bbTimer = self.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
                InputCallback callback = self.tcss[i];
                //当前输入不符合条件
                switch (callback.waitType)
                {
                    case FuzzyInputType.OR:
                        if ((op & callback.OP) == 0) continue;
                        break;
                    case FuzzyInputType.AND:
                        if ((op & callback.OP) != callback.OP) continue;
                        break;
                    case FuzzyInputType.Hold:
                        //蓄力中，一直拉后，不包含拉后指令了，判断退出蓄力协程
                        if ((op & callback.OP) != 0) continue;
                        break;
                }

                callback.SetResult(new WaitInput() { frame = bbTimer.GetNow(), Error = WaitTypeError.Success });
                self.tcss.Remove(callback);
            }
        }

        public static async ETTask<WaitInput> Wait(this InputWait self, long OP, int waitType)
        {
            InputCallback tcs = InputCallback.Create(OP, waitType);
            self.tcss.Add(tcs);

            void CancelAction()
            {
                self.tcss.Remove(tcs);
                tcs.SetResult(new WaitInput() { Error = WaitTypeError.Cancel });
                tcs.Recycle();
            }

            WaitInput ret;
            try
            {
                self.Token?.Add(CancelAction);
                ret = await tcs.Task;
            }
            finally
            {
                self.Token.Remove(CancelAction);
            }

            return ret;
        }

        private static async ETTask<WaitInput> Wait(this InputWait self, long OP, int waitType, int waitFrame)
        {
            InputCallback tcs = InputCallback.Create(OP, waitType);
            self.tcss.Add(tcs);

            async ETTask WaitTimeOut()
            {
                // n帧内输入有效(sf6训练场还有该标准速度的选项，调整Hertz后，犹豫期也会跟着改变)
                BBTimerComponent bbTimer = self.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
                await bbTimer.WaitAsync(waitFrame, self.Token);
                if (self.Token.IsCancel() || tcs.IsDisposed)
                {
                    return;
                }

                self.tcss.Remove(tcs);
                tcs.SetResult(new WaitInput() { Error = WaitTypeError.Timeout });
                tcs.Recycle();
            }

            WaitTimeOut().Coroutine();

            void CancelAction()
            {
                self.tcss.Remove(tcs);
                tcs.SetResult(new WaitInput() { Error = WaitTypeError.Cancel });
                tcs.Recycle();
            }

            WaitInput ret;
            try
            {
                self.Token?.Add(CancelAction);
                ret = await tcs.Task;
            }
            finally
            {
                self.Token?.Remove(CancelAction);
            }

            return ret;
        }

        private static async ETTask InputCheckCor(this InputWait self, BBInputHandler handler, ETCancellationToken token)
        {
            self.runningHandlers.Add(handler.GetInputType());
            Status ret = await handler.Handle(self.GetParent<TimelineComponent>().GetParent<Unit>(), token);
            self.runningHandlers.Remove(handler.GetInputType());

            if (ret is Status.Success)
            {
                BBTimerComponent bbTimer = self.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
                InputBuffer buffer = InputBuffer.Create(handler, bbTimer.GetNow(), bbTimer.GetNow() + 5);
                self.AddBuffer(buffer);
            }
        }

        public static void AddBuffer(this InputWait self, InputBuffer buffer)
        {
            while (self.bufferQueue.Count >= InputWait.MaxStack)
            {
                InputBuffer oldBuffer = self.bufferQueue.Dequeue();
                oldBuffer.Recycle();
            }

            self.bufferQueue.Enqueue(buffer);
        }

        private static void UpdateBuffer(this InputWait self)
        {
            BBTimerComponent bbTimer = self.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();

            int count = self.bufferQueue.Count;
            while (count-- > 0)
            {
                InputBuffer buffer = self.bufferQueue.Dequeue();
                BBInputHandler handler = buffer.handler;

                if (bbTimer.GetNow() > buffer.lastedFrame)
                {
                    if (self.bufferDict.ContainsKey(handler.GetInputType()))
                    {
                        self.bufferDict[handler.GetInputType()] = false;
                    }

                    buffer.Recycle();
                    continue;
                }

                self.bufferQueue.Enqueue(buffer);
                if (!self.bufferDict.ContainsKey(handler.GetInputType()))
                {
                    self.bufferDict.Add(handler.GetInputType(), true);
                }
                else
                {
                    self.bufferDict[handler.GetInputType()] = true;
                }
            }
        }

        public static bool CheckInput(this InputWait self, string inputType)
        {
            self.bufferDict.TryGetValue(inputType, out bool value);
            return value;
        }
    }
}