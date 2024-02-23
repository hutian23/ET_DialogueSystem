using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof (DialogueDispatcherComponent))]
    [FriendOf(typeof (BBWait))]
    public static class BBWaitSystem
    {
        public static void CheckCor(this BBWait self)
        {
            // foreach (var VARIABLE in COLLECTION)
            {
                
            }
        }
        
        // https://www.zhihu.com/question/36951135/answer/69880133
        public static void Notify(this BBWait self, long OP)
        {
            var tmp = new List<InputCallback>();
            self.tcss.ForEach(inputCallback =>
            {
                //当前输入不符合条件
                if ((inputCallback.OP & OP) == 0) return;

                TODTimerComponent timerComponent = self.GetParent<BBInputComponent>().GetComponent<TODTimerComponent>();
                inputCallback.SetResult(new WaitInput() { frame = timerComponent.GetNow(), Error = WaitTypeError.Success });
                tmp.Add(inputCallback);
            });
            for (int i = 0; i < tmp.Count; i++)
            {
                self.tcss.Remove(tmp[i]);
            }
        }

        /// <summary>
        /// 等待起手指令输入
        /// 比如一些起手的指令 236P 第一个2的等待帧数无限长
        /// </summary>
        public static async ETTask<WaitInput> Wait(this BBWait self, long OP)
        {
            InputCallback tcs = new();
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
        /// <param name="self"></param>
        /// <param name="OP"></param>
        /// <param name="waitFrame"></param>
        /// <returns></returns>
        public static async ETTask<WaitInput> Wait(this BBWait self, long OP, int waitFrame)
        {
            InputCallback tcs = new();
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