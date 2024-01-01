using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof (Scene))]
    public class TODEventSystem: Entity, IAwake, IDestroy, ILoad
    {
        [StaticField]
        public static TODEventSystem Instance;

        public readonly Dictionary<string, BehaviorHandler> Behaviors = new();
        public readonly Dictionary<string, CheckerHandler> checkers = new();
        public readonly Dictionary<string, NodeCheckerHandler> nodeCheckers = new();
    }
}