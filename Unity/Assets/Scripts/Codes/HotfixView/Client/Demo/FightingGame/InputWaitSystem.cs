using System;
using System.Collections.Generic;
using System.Linq;

namespace ET.Client
{
    public static class InputWaitSystem
    {
        // https://www.zhihu.com/question/36951135/answer/69880133
        public static void Notify(this InputWait self)
        {
        }

        //默认犹豫期为5帧
        public static async ETTask<InputBuffer> Wait(this InputWait self, int op, ETCancellationToken cancellationToken, int waitType, int waitFrame = 5)
        {
            await ETTask.CompletedTask;
            return new InputBuffer();
        }
    }
}