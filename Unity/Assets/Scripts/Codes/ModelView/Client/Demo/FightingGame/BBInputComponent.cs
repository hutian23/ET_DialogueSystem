using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf]
    public class BBInputComponent: Entity, IAwake, IDestroy, IUpdate, ILoad
    {
        public long timer;

        public int maxStackSize = 60;

        //按键与按下按键的帧号的映射，超过 n 帧判定为过期，这个按键没有按下
        public Dictionary<int, long> pressDict = new();
    }

    public class InputInfo
    {
        public long frame;
        public long ops;
        public long lastedFrame; //持续时间
        //TODO 考虑转向问题，是否需要翻转指令?

        public static InputInfo Create(long _frame, long _ops)
        {
            InputInfo info = ObjectPool.Instance.Fetch<InputInfo>();
            info.frame = _frame;
            info.ops = _ops;
            info.lastedFrame = 0;
            return info;
        }

        public void Recyle()
        {
            this.frame = 0;
            this.ops = 0;
            this.lastedFrame = 0;
        }
    }
}