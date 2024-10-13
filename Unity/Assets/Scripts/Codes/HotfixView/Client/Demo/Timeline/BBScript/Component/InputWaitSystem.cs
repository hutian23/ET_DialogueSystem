using System;
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
                self.Ops = ops;

                self.UpdatekeyHistory(self.Ops);
                self.Notify(self.Ops);
                //检测执行完的输入协程，重新执行
                foreach (BBInputHandler handler in self.handlers)
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

        public class InputWaitAwakeSystem: AwakeSystem<InputWait>
        {
            protected override void Awake(InputWait self)
            {
                self.AddComponent<BBTimerComponent>();
                self.RegistKeyHistory();
            }
        }

        public class InputWaitDestroySystem: DestroySystem<InputWait>
        {
            protected override void Destroy(InputWait self)
            {
                self.Init();
                self.PressedDict.Clear();
            }
        }

        public static void Cancel(this InputWait self)
        {
            #region Init

            self.GetComponent<BBTimerComponent>().Remove(ref self.Timer);
            self.Ops = 0;

            //InputHandler会在所有行为初始化时注册到handlers中，所以启动关闭输入窗口不能清空该列表
            self.runningHandlers.Clear();
            self.bufferDict.Clear();

            self.Token?.Cancel();
            self.tcss.ForEach(tcs => tcs.Recycle());
            self.tcss.Clear();
            self.Token = new ETCancellationToken();

            self.bufferQueue.ForEach(buffer => buffer.Recycle());
            self.bufferQueue.Clear();

            #endregion
        }

        public static void Init(this InputWait self)
        {
            self.Cancel();
            self.handlers.Clear();
            self.GetComponent<BBTimerComponent>().ReLoad();
        }

        public static void Reload(this InputWait self)
        {
            self.Cancel();
            //启动定时器
            self.Timer = self.GetComponent<BBTimerComponent>().NewFrameTimer(BBTimerInvokeType.BBInputTimer, self);
        }

        public static void ReloadTimer(this InputWait self)
        {
            self.Timer = self.GetComponent<BBTimerComponent>().NewFrameTimer(BBTimerInvokeType.BBInputTimer, self);
        }

        public static void CancelTimer(this InputWait self)
        {
            self.GetComponent<BBTimerComponent>().Remove(ref self.Timer);
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

                //回调检查委托
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