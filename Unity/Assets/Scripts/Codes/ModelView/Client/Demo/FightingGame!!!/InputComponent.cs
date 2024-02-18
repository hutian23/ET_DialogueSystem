using System.Collections.Generic;

namespace ET.Client
{
    public class InputComponent: Entity, IAwake, IDestroy
    {
        public Queue<InputInfo> infos = new();
    }

    public struct InputInfo
    {
        public long frame;
        public List<int> op;
    }

    [UniqueId(0, 1000)]
    public static class FtgOperaType
    {
        public const int None = 0;
        public const int DOWNLEFT = 1;
        public const int DOWN = 2;
        public const int DOWNRIGHT = 3;
        public const int LEFT = 4;
        public const int MIDDLE = 5;
        public const int RIGHT = 6;
        public const int UPLEFT = 7;
        public const int UP = 8;
        public const int UPRIGHT = 9;
        
        public const int LIGHTPUNCH = 10;
        public const int LIGHTKICK = 11;
        public const int MIDDLEPUNCH = 12;
        public const int MIDDLEKICK = 13;
        public const int HEAVYPUNCH = 14;
        public const int HEAVYKICK = 15;
    }
}