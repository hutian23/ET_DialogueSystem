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
}