using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 输入模块
    /// </summary>
    [ComponentOf(typeof(Unit))]
    public class InputComponent: Entity, IAwake, IDestroy, IFrameUpdate
    {
        public long curOP;
        public const int MaxStack = 100;
        public Queue<InputInfo> infoQueue = new();
        public Queue<string> handleQueue = new();
        
        public Dictionary<string, long> BufferDict = new(); // 记录了输入缓冲有效的最大帧号
        public Dictionary<long, long> PressedDict = new();
        public Dictionary<long, bool> IsPressingDict = new();
        public Dictionary<long, long> PressingDict = new();
    }
    
    public struct InputInfo
    {
        public long frame;
        public long op;
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

        public const int X = 2 << 9;
        public const int A = 2 << 10;
        public const int Y = 2 << 11;
        public const int B = 2 << 12;
        public const int RB = 2 << 13;
        public const int RT = 2 << 14;
        public const int LB = 2 << 15;
        public const int LT = 2 << 16;
    }
}