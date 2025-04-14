using System.Collections.Generic;

namespace ET.Client
{
    //用于取消到特定的动作
    [ComponentOf(typeof(BBParser))]
    public class TargetCancelComponent : Entity, IAwake, IDestroy
    {
        public long timer;
        public HashSet<string> Options = new();
    }
}