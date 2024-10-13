using System;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 输入模块
    /// </summary>
    [ComponentOf(typeof (TimelineComponent))]
    public class InputWait: Entity, IAwake, IDestroy
    {
        public long inputTimer;
        public long Ops;
        public bool IsOpen; //输入窗口中

        //BBScript中init协程中注册到这里
        public HashSet<BBInputHandler> handlers = new();
        public HashSet<string> runningHandlers = new();
        public Dictionary<string, bool> bufferDict = new();

        public ETCancellationToken Token = new();
        public List<InputCallback> tcss = new();

        public Queue<InputBuffer> bufferQueue = new();
        public const int MaxStack = 30;

        public Dictionary<int, long> PressedDict = new();
        public Dictionary<int, long> ReleasedDict = new();
    }

    public struct AfterUpdateInput
    {
        public long OP;
        public long instanceId;
    }

    public class InputBuffer
    {
        public BBInputHandler handler;
        public long startFrame;
        public long lastedFrame;

        public static InputBuffer Create(BBInputHandler handler, long startFrame, long lastedFrame)
        {
            InputBuffer buffer = ObjectPool.Instance.Fetch<InputBuffer>();
            buffer.handler = handler;
            buffer.startFrame = startFrame;
            buffer.lastedFrame = lastedFrame;
            return buffer;
        }

        public void Recycle()
        {
            handler = null;
            startFrame = 0;
            lastedFrame = 0;
            ObjectPool.Instance.Recycle(this);
        }
    }

    public class InputCallback
    {
        public bool IsDisposed
        {
            get
            {
                return tcs == null;
            }
        }

        public ETTask<WaitInput> Task => tcs;

        public void SetResult(WaitInput wait)
        {
            var t = tcs;
            tcs = null;
            t.SetResult(wait);
        }

        public void SetException()
        {
            var t = tcs;
            tcs = null;
            t.SetResult(new WaitInput() { Error = WaitTypeError.Destroy });
        }

        public long OP;
        public int waitType;
        public Func<bool> checkFunc;

        private ETTask<WaitInput> tcs;

        public static InputCallback Create(long OP, int waitType, Func<bool> checkFunc)
        {
            InputCallback callback = ObjectPool.Instance.Fetch<InputCallback>();
            callback.OP = OP;
            callback.waitType = waitType;
            callback.tcs = ETTask<WaitInput>.Create(true);
            callback.checkFunc = checkFunc;
            return callback;
        }

        public void Recycle()
        {
            OP = 0;
            waitType = 0;
            tcs = null;
            checkFunc = null;
            ObjectPool.Instance.Recycle(this);
        }
    }

    public struct WaitInput: IWaitType
    {
        //缓冲完成条件那一帧的输入
        //把按键都抽象成0和1，按下LP 1,没按下 0
        //把所有输入的状态码拼在一起，变成一个64位的整形，如下
        //eg 0000 0000 0000 0000 0000 0100 0000 0100 当前帧按下LP和下
        public long frame;
        public long OP;
        public int Error { get; set; }
    }

    public static class FuzzyInputType
    {
        public const int None = 0;
        public const int AND = 1;
        public const int OR = 2;
        public const int Hold = 3;
        public const int IsPressed = 4; //长按一个按键，只会执行一次技能
    }

    public static class BBOperaType
    {
        public const int None = 0;
        public const int DOWNLEFT = 2 << 0;
        public const int DOWN = 2 << 1;
        public const int DOWNRIGHT = 2 << 2;
        public const int LEFT = 2 << 3;
        public const int MIDDLE = 2 << 4;
        public const int RIGHT = 2 << 5;
        public const int UPLEFT = 2 << 6;
        public const int UP = 2 << 7;
        public const int UPRIGHT = 2 << 8;

        public const int LIGHTPUNCH = 2 << 9;
        public const int LIGHTKICK = 2 << 10;
        public const int MIDDLEPUNCH = 2 << 11;
        public const int MIDDLEKICK = 2 << 12;
        public const int HEAVYPUNCH = 2 << 13;
        public const int HEAVYKICK = 2 << 14;
    }
}