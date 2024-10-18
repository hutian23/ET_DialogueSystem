using System;
using Sirenix.Utilities;

namespace ET.Client
{
    [FriendOf(typeof (InputWait))]
    public static class InputWaitSystem
    {
        [Invoke(BBTimerInvokeType.BBInputHandleTimer)]
        [FriendOf(typeof (BBInputComponent))]
        [FriendOf(typeof (InputWait))]
        public class BBInputHandleTimer: BBTimer<InputWait>
        {
            protected override void Run(InputWait self)
            {
                self.Ops = BBInputComponent.Instance.Ops;
                //1. 更新按键历史
                self.UpdatekeyHistory(self.Ops);
                //2. 更新缓冲区
                self.UpdateBuffer();
            }
        }

        [Invoke(BBTimerInvokeType.BBInputNotifyTimer)]
        [FriendOf(typeof (InputWait))]
        public class BBInputNotifyTimer: BBTimer<InputWait>
        {
            protected override void Run(InputWait self)
            {
                //输入窗口打开,处理预输入逻辑
                self.Notify(self.Ops);
                //检测执行完的输入协程，重新执行
                foreach (BBInputHandler handler in self.handlers)
                {
                    if (!self.runningHandlers.Contains(handler.GetInputType()))
                    {
                        self.InputCheckCor(handler, self.Token).Coroutine();
                    }
                }
            }
        }

        public class InputWaitAwakeSystem: AwakeSystem<InputWait>
        {
            protected override void Awake(InputWait self)
            {
                self.AddComponent<BBTimerComponent>();
            }
        }

        public static void Reload(this InputWait self)
        {
            #region Init

            self.GetComponent<BBTimerComponent>().Remove(ref self.inputHandleTimer);
            self.GetComponent<BBTimerComponent>().Remove(ref self.inputNotifyTimer);
            self.GetComponent<BBTimerComponent>().ReLoad();
            self.Ops = 0;

            self.handlers.Clear();
            self.runningHandlers.Clear();

            self.Token?.Cancel();
            self.tcss.ForEach(tcs => tcs.Recycle());
            self.tcss.Clear();
            self.Token = new ETCancellationToken();

            self.bufferDict.Clear();
            self.bufferQueue.ForEach(buffer => buffer.Recycle());
            self.bufferQueue.Clear();

            self.PressedDict.Clear();

            #endregion

            self.RegistKeyHistory();

            //启动定时器
            self.inputHandleTimer = self.GetComponent<BBTimerComponent>().NewFrameTimer(BBTimerInvokeType.BBInputHandleTimer, self);
        }

        #region KeyHistory

        private static void RegistKeyHistory(this InputWait self)
        {
            self.PressedDict.Add(BBOperaType.LIGHTPUNCH, long.MaxValue);
            self.PressedDict.Add(BBOperaType.LIGHTKICK, long.MaxValue);
            self.PressedDict.Add(BBOperaType.MIDDLEPUNCH, long.MaxValue);
            self.PressedDict.Add(BBOperaType.HEAVYKICK, long.MaxValue);
            self.PressedDict.Add(BBOperaType.HEAVYPUNCH, long.MaxValue);
        }

        private static void UpdatekeyHistory(this InputWait self, long ops)
        {
            self.HandleKeyInput(ops, BBOperaType.LIGHTPUNCH);
            self.HandleKeyInput(ops, BBOperaType.LIGHTKICK);
            self.HandleKeyInput(ops, BBOperaType.MIDDLEPUNCH);
            self.HandleKeyInput(ops, BBOperaType.MIDDLEKICK);
            self.HandleKeyInput(ops, BBOperaType.HEAVYPUNCH);
            self.HandleKeyInput(ops, BBOperaType.HEAVYKICK);
        }

        private static void HandleKeyInput(this InputWait self, long ops, int operaType)
        {
            BBTimerComponent bbTimer = self.GetComponent<BBTimerComponent>();
            bool ret = (ops & operaType) != 0;
            //按住，倾刻炼化
            if (ret)
            {
                if (self.PressedDict[operaType] > bbTimer.GetNow())
                {
                    self.PressedDict[operaType] = bbTimer.GetNow();
                }
            }
            else
            {
                self.PressedDict[operaType] = long.MaxValue;
            }
        }

        public static bool WasPressedThisFrame(this InputWait self, int operaType)
        {
            return self.PressedDict[operaType] == self.GetComponent<BBTimerComponent>().GetNow();
        }

        #endregion

        //https://www.zhihu.com/question/36951135/answer/69880133
        private static void Notify(this InputWait self, long op)
        {
            for (int i = 0; i < self.tcss.Count; i++)
            {
                BBTimerComponent bbTimer = self.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
                InputCallback callback = self.tcss[i];
                //1. 当前输入不符合条件
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

                if (callback.checkFunc != null && !callback.checkFunc.Invoke()) continue;

                //2. 不同技能可能有不同的判定逻辑
                callback.SetResult(new WaitInput() { frame = bbTimer.GetNow(), Error = WaitTypeError.Success, OP = op });
                self.tcss.Remove(callback);
            }
        }

        public static async ETTask<WaitInput> Wait(this InputWait self, long OP, int waitType, Func<bool> checkFunc = null)
        {
            InputCallback tcs = InputCallback.Create(OP, waitType, checkFunc);
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

        private static void AddBuffer(this InputWait self, InputBuffer buffer)
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

        public static void StartNotifyTimer(this InputWait self)
        {
            BBTimerComponent bbTimer = self.GetComponent<BBTimerComponent>();
            bbTimer.Remove(ref self.inputNotifyTimer);
            self.inputNotifyTimer = bbTimer.NewFrameTimer(BBTimerInvokeType.BBInputNotifyTimer, self);
        }

        public static void CancelNotifyTimer(this InputWait self)
        {
            BBTimerComponent bbTimer = self.GetComponent<BBTimerComponent>();
            bbTimer.Remove(ref self.inputNotifyTimer);
        }
    }
}