namespace ET.Client
{
    [UniqueId(0, 1000)]
    public static class OperaType
    {
        public const int None = 0;

        public const int RightMoveWasPressed = 1;
        public const int RightMovePressing = 2;
        public const int RightMoveReleased = 3;

        public const int LeftMoveWasPressed = 4;
        public const int LeftMovePressing = 5;
        public const int LeftMoveReleased = 6;

        public const int DownWasPressed = 7;
        public const int DownPressing = 8;
        public const int DownReleased = 9;

        public const int UpWasPressed = 10;
        public const int UpPressing = 11;
        public const int UpReleased = 12;
    }
}