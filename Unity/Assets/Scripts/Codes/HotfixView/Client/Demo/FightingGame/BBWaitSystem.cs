using Sirenix.Utilities;

namespace ET.Client
{
    [FriendOf(typeof (DialogueDispatcherComponent))]
    [FriendOf(typeof (BBWait))]
    public static class BBWaitSystem
    {
        public class BBWaitAwakeSystem: AwakeSystem<BBWait>
        {
            protected override void Awake(BBWait self)
            {
                self.Init();
            }
        }

        public class BBWaitLoadSystem: LoadSystem<BBWait>
        {
            protected override void Load(BBWait self)
            {
                self.Init();
            }
        }

        /// <summary>
        /// 取消所有招式检测协程
        /// </summary>
        public static void Cancel(this BBWait self)
        {
            self.token?.Cancel();
            self.tcss.ForEach(tcs => { tcs.SetResult(new WaitInput() { Error = WaitTypeError.Cancel }); });
            self.tcss.Clear();
        }

        public static void Init(this BBWait self)
        {
            self.Cancel();
            self.token = new ETCancellationToken();
            DialogueDispatcherComponent.Instance.BBCheckHandlers.Values.ForEach(handler => { self.CheckCor(handler).Coroutine(); });
        }

        private static async ETTask CheckCor(this BBWait self, BBCheckHandler handler)
        {
            while (true)
            {
                if (self.token.IsCancel()) return;
                Unit unit = self.GetParent<BBInputComponent>().GetParent<DialogueComponent>().GetParent<Unit>();
                await handler.Handle(unit, self.token);
                if (self.token.IsCancel()) return;
                await TimerComponent.Instance.WaitFrameAsync(self.token);
            }
        }

        // https://www.zhihu.com/question/36951135/answer/69880133
        public static void Notify(this BBWait self, long OP)
        {
            for (int i = 0; i < self.tcss.Count; i++)
            {
                InputCallback inputCallback = self.tcss[i];
                //当前输入不符合条件
                switch (inputCallback.waitType)
                {
                    case FuzzyInputType.OR:
                        if ((OP & inputCallback.OP) == 0) continue;
                        break;
                    case FuzzyInputType.AND:
                        if ((OP & inputCallback.OP) != inputCallback.OP) continue;
                        break;
                }

                TODTimerComponent timerComponent = self.GetParent<BBInputComponent>().GetComponent<TODTimerComponent>();
                inputCallback.SetResult(new WaitInput() { frame = timerComponent.GetNow(), Error = WaitTypeError.Success });
                self.tcss.Remove(inputCallback);
            }
        }

        /// <summary>
        /// 等待起手指令输入
        /// 比如一些起手的指令 236P 第一个2的等待帧数无限长
        /// </summary>
        public static async ETTask<WaitInput> Wait(this BBWait self, long OP, int waitType)
        {
            InputCallback tcs = new() { OP = OP, waitType = waitType };
            self.tcss.Add(tcs);

            void CancelAction()
            {
                self.tcss.Remove(tcs);
                tcs.SetResult(new WaitInput() { Error = WaitTypeError.Cancel });
            }

            WaitInput ret;
            try
            {
                self.token?.Add(CancelAction);
                ret = await tcs.Task;
            }
            finally
            {
                self.token.Remove(CancelAction);
            }

            return ret;
        }

        /// <summary>
        /// 按键犹豫期
        /// </summary>
        public static async ETTask<WaitInput> Wait(this BBWait self, long OP, int waitType, int waitFrame)
        {
            InputCallback tcs = new() { OP = OP, waitType = waitType };
            self.tcss.Add(tcs);

            async ETTask WaitTimeOut()
            {
                // n帧内输入有效(sf6训练场还有该标准速度的选项，调整timeScale后，犹豫期也会跟着改变)
                TODTimerComponent timerComponent = self.GetParent<BBInputComponent>().GetComponent<TODTimerComponent>();
                await timerComponent.WaitAsync(waitFrame, self.token);
                if (self.token.IsCancel())
                {
                    return;
                }

                if (tcs.IsDisposed)
                {
                    return;
                }

                self.tcss.Remove(tcs);
                tcs.SetResult(new WaitInput() { Error = WaitTypeError.Timeout });
            }

            WaitTimeOut().Coroutine();

            void CancelAction()
            {
                self.tcss.Remove(tcs);
                tcs.SetResult(new WaitInput() { Error = WaitTypeError.Cancel });
            }

            WaitInput ret;
            try
            {
                self.token?.Add(CancelAction);
                ret = await tcs.Task;
            }
            finally
            {
                self.token?.Remove(CancelAction);
            }

            return ret;
        }
    }
}