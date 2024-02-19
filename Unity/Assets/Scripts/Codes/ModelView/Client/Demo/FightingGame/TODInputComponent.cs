using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf]
    public class TODInputComponent: Entity, IAwake, IDestroy,IUpdate, ILoad
    {
        public long timer;
        public Queue<InputInfo> infos = new();
    }

    public struct InputInfo
    {
        public long frame;
        public int ops;
    }
}