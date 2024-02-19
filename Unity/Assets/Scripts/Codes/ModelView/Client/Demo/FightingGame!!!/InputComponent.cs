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
        public int ops;
    }
}