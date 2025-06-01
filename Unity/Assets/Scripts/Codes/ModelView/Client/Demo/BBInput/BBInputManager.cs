using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class BBInputManager: Entity, IAwake, IDestroy, IUpdate, ILoad
    {
        [StaticField]
        public static BBInputManager Instance;

        public long Ops;

        public Dictionary<int, bool> WasPressedDict = new();
    }
}