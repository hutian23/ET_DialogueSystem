using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf]
    public class BBInputComponent: Entity, IAwake, IDestroy, IUpdate, ILoad
    {
        [StaticField]
        public static BBInputComponent Instance;

        public long Ops;

        public Dictionary<int, bool> WasPressedDict = new();
    }

    public struct UpdateInputCallback
    {
        public long Ops;
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