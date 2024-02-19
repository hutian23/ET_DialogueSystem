namespace ET.Client
{
    [ComponentOf]
    public class InputWait: Entity, IAwake, IDestroy, ILoad
    {
        // public Dictionary<Type, object> tcss = new();
        // public List<InputResultCallback> resultCallbacks = new();
    }

    public struct InputBuffer : IWaitType
    {
        //缓冲完成条件那一帧的输入
        //把按键都抽象成0和1，按下LP 1,没按下 0
        //把所有输入的状态码拼在一起，变成一个64位的整形，如下
        //eg 0000 0000 0000 0000 0000 0100 0000 0100 当前帧按下LP和下
        public long inputInfo; 
        public long frameID;   //帧号
        public int Error { get; set; }
    }

    public static class TODOperaType
    {
        public const int None = 0;
        public const int DOWNLEFT = 2 << 1;
        public const int DOWN = 2 << 2;
        public const int DOWNRIGHT = 2 << 3;
        public const int LEFT = 2 << 4;
        public const int MIDDLE = 2 << 5;
        public const int RIGHT = 2 << 6;
        public const int UPLEFT = 2 << 7;
        public const int UP = 2 << 8;
        public const int UPRIGHT = 2 << 9;
        
        public const int LIGHTPUNCH = 2 << 10;
        public const int LIGHTKICK = 2 << 11;
        public const int MIDDLEPUNCH = 2 << 12;
        public const int MIDDLEKICK = 2 << 13;
        public const int HEAVYPUNCH = 2 << 14;
        public const int HEAVYKICK = 2 << 15;
    }
}